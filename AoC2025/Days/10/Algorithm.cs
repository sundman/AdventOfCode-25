

using System;
using System.Linq;

public static class BranchAndBoundMinSum
{
    /// <summary>
    /// Minimize sum(x) subject to A x = b, x >= 0, x integer (pure C# branch-and-bound).
    /// Returns the globally minimal nonnegative integer solution, or null if infeasible.
    /// 
    /// Parameters:
    ///  - timeLimitMs: optional wall clock limit (0 = no limit)
    ///  - nodeLimit:   optional limit of explored nodes (0 = no limit)
    /// 
    /// Notes:
    ///  - Uses only int with checked arithmetic. If you risk overflow, switch all ints to long.
    ///  - Works for square, overdetermined, and underdetermined systems.
    /// </summary>
    public static int[] SolveMinSum(int[,] A, int[] b, long nodeLimit = 0)
    {
        int m = A.GetLength(0);
        int n = A.GetLength(1);
        if (b.Length != m) throw new ArgumentException("b length must equal row count of A.");

        // Variable ordering heuristic: descending sum of |column|
        var scores = new (int col, long score)[n];
        for (int j = 0; j < n; j++)
        {
            long s = 0;
            for (int i = 0; i < m; i++) s += Math.Abs(A[i, j]);
            scores[j] = (j, s);
        }
        Array.Sort(scores, (x, y) => y.score.CompareTo(x.score));
        int[] order = scores.Select(p => p.col).ToArray();

        // Permute columns of A -> Aord by 'order'
        int[,] Aord = new int[m, n];
        for (int i = 0; i < m; i++)
            for (int k = 0; k < n; k++)
                Aord[i, k] = A[i, order[k]];

        // Residuals r = b - sum_{assigned} A * x
        int[] residual = (int[])b.Clone();

        // Best solution tracking
        int bestSum = int.MaxValue;
        int[] bestXOrd = null;

        // Current branch assignment in ordered coordinates
        int[] xOrd = new int[n];

        // Optional limits
        long explored = 0;

        // Precompute per-row: max positive coefficient among all columns (used for LB on remaining sum)
        // and we’ll recompute among remaining columns at each depth to tighten bound.
        int[] rowMaxPosAll = new int[m];
        for (int i = 0; i < m; i++)
        {
            int mx = 0;
            for (int j = 0; j < n; j++)
                if (Aord[i, j] > 0 && Aord[i, j] > mx) mx = Aord[i, j];
            rowMaxPosAll[i] = mx; // may be 0 if no positive coeff in row
        }

        void Dfs(int idx, int sumSoFar)
        {
            // limits
            if (nodeLimit > 0 && explored >= nodeLimit) return;

            explored++;
            if (sumSoFar >= bestSum) return; // objective pruning

            if (idx == n)
            {
                // Check exact satisfaction
                for (int i = 0; i < m; i++) if (residual[i] != 0) return;
                bestSum = sumSoFar;
                bestXOrd = (int[])xOrd.Clone();
                return;
            }

            // Compute safe bounds L and U for xOrd[idx]
            int L = 0;
            int U = int.MaxValue;
            bool anyFiniteU = false;

            for (int i = 0; i < m; i++)
            {
                int a = Aord[i, idx];
                if (a == 0) continue;

                // Determine if remaining columns (k > idx) are all >=0 or all <=0 in this row
                bool remAllNonNeg = true, remAllNonPos = true;
                for (int k = idx + 1; k < n; k++)
                {
                    int ak = Aord[i, k];
                    if (ak < 0) remAllNonNeg = false;
                    if (ak > 0) remAllNonPos = false;
                    if (!remAllNonNeg && !remAllNonPos) break;
                }

                int r = residual[i];

                if (a > 0)
                {
                    // If remaining can't decrease (all >= 0), then a*x <= r  -> U bound
                    if (remAllNonNeg)
                    {
                        int cap = FloorDiv(r, a);
                        if (cap < U) { U = cap; anyFiniteU = true; }
                    }
                    // If remaining can't increase (all <= 0), then a*x >= r  -> L bound
                    if (remAllNonPos)
                    {
                        int lb = CeilDiv(r, a);
                        if (lb > L) L = lb;
                    }
                }
                else // a < 0
                {
                    // If remaining can't increase (all <= 0), then a*x <= r -> since a<0 => x >= ceil(r/a)
                    if (remAllNonPos)
                    {
                        int lb = CeilDiv(r, a);
                        if (lb > L) L = lb;
                    }
                    // If remaining can't decrease (all >= 0), then a*x >= r -> since a<0 => x <= floor(r/a)
                    if (remAllNonNeg)
                    {
                        int cap = FloorDiv(r, a);
                        if (cap < U) { U = cap; anyFiniteU = true; }
                    }
                }
            }

            if (L < 0) L = 0;
            if (!anyFiniteU) U = Math.Max(U, 0); // no finite cap -> try small values (0 upward)
            if (U < L) return; // infeasible bounds

            // Quick necessary feasibility checks using remaining columns (k >= idx)
            for (int i = 0; i < m; i++)
            {
                bool allNonNegRem = true, allNonPosRem = true;
                for (int k = idx; k < n; k++)
                {
                    int ak = Aord[i, k];
                    if (ak < 0) allNonNegRem = false;
                    if (ak > 0) allNonPosRem = false;
                    if (!allNonNegRem && !allNonPosRem) break;
                }
                if (allNonNegRem && residual[i] < 0) return; // cannot increase to fix negative
                if (allNonPosRem && residual[i] > 0) return; // cannot decrease to fix positive
            }

            // Extra lower bound on remaining sum:
            // For rows where remaining columns are all nonnegative, we must achieve residual[i] >= 0 using only nonneg coeffs.
            // The most sum-efficient way is to put all mass into the largest positive coeff among remaining columns.
            int lbExtra = 0;
            for (int i = 0; i < m; i++)
            {
                int r = residual[i];
                // compute max positive coeff among remaining columns (k >= idx)
                int maxPos = 0;
                for (int k = idx; k < n; k++)
                    if (Aord[i, k] > 0 && Aord[i, k] > maxPos) maxPos = Aord[i, k];

                // If remaining columns are all >=0 and residual is positive, we need at least ceil(r / maxPos) units of sum
                bool allNonNegRem = true;
                for (int k = idx; k < n; k++)
                    if (Aord[i, k] < 0) { allNonNegRem = false; break; }

                if (allNonNegRem && r > 0 && maxPos > 0)
                {
                    int need = CeilDiv(r, maxPos);
                    if (need > lbExtra) lbExtra = need;
                }
            }
            if (sumSoFar + lbExtra >= bestSum) return; // bound-based pruning

            // Try small values first (0..U) to reach good solutions early
            for (int val = L; val <= U; val++)
            {
                if (sumSoFar + val >= bestSum) break; // objective pruning on the fly

                // Apply val
                bool ok = true;
                try
                {
                    for (int i = 0; i < m; i++)
                    {
                        checked { residual[i] -= Aord[i, idx] * val; }
                    }
                }
                catch (OverflowException) { ok = false; }

                if (ok)
                {
                    xOrd[idx] = val;
                    Dfs(idx + 1, sumSoFar + val);
                }

                // Revert
                for (int i = 0; i < m; i++)
                {
                    residual[i] += Aord[i, idx] * val; // revert (unchecked fine)
                }
            }
        }

        Dfs(0, 0);

        if (bestXOrd == null)
        {
            Console.WriteLine($"Giving up at a bestSum of {bestSum}");
            return null;
        }

        // Map back to original variable order
        int[] x = new int[n];
        for (int k = 0; k < n; k++) x[order[k]] = bestXOrd[k];
        return x;
    }

    // Floor division toward -∞ (mathematical), safe for mixed signs
    private static int FloorDiv(int a, int b)
    {
        if (b == 0) throw new DivideByZeroException();
        int q = a / b;
        int r = a % b;
        if (r != 0 && ((r > 0) ^ (b > 0))) q--;
        return q;
    }

    // Ceil division toward +∞ (mathematical), safe for mixed signs
    private static int CeilDiv(int a, int b)
    {
        if (b == 0) throw new DivideByZeroException();
        int q = a / b;
        int r = a % b;
        if (r != 0 && !((r > 0) ^ (b > 0))) q++;
        return q;
    }
}
