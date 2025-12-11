using Google.OrTools.Sat;

public class OrToolsMinSumOnly
{
    public OrToolsMinSumOnly()
    {
        // NOTE!! This brings runtime down from 3000ms to 100ms. Guess too many easy tasks
        solver = new CpSolver();
        var parameters = "num_search_workers:1"; 
        solver.StringParameters = parameters;
    }

    private CpSolver solver;

    public int? SolveMinSumOnly(int[,] A, int[] b, int[] upperBoundPerVar)
    {
        int m = A.GetLength(0);
        int n = A.GetLength(1);
        if (b.Length != m) throw new ArgumentException("b length must equal row count of A.");

        var model = new CpModel();

        int[] U = upperBoundPerVar;

        // Decision variables
        var x = new IntVar[n];
        for (int j = 0; j < n; j++)
        {
            x[j] = model.NewIntVar(0, U[j], $"x_{j}");
        }

        // Constraints: Σ_j A[i,j] * x_j == b[i]
        for (int i = 0; i < m; i++)
        {
            LinearExpr row = LinearExpr.Constant(0);
            for (var j = 0; j < n; j++)
            {
                row += A[i, j] * x[j];
            }
            model.Add(row == b[i]);
        }

        // Objective: minimize Σ_j x_j
        var sumExpr = LinearExpr.Sum(x);
        model.Minimize(sumExpr);



        var status = solver.Solve(model);

        return (int)solver.ObjectiveValue;
    }
}
