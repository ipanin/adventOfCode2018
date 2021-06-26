using System;
using System.Linq;
using System.Reflection;

namespace Aoc
{
    public static class Program
    {
        
        public static ISolver[] GetSolvers() 
        {
            var solvers = Assembly.GetEntryAssembly()!.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass 
                            && !t.GetTypeInfo().IsAbstract
                            && typeof(ISolver).IsAssignableFrom(t))
                .OrderBy(t => t.FullName);
            
            return solvers.Select(t => Activator.CreateInstance(t) as ISolver).ToArray();
        }

        public static void Main()
        {
            var solvers = GetSolvers();
            
            foreach (var solver in solvers) {
                solver.Solve();
            }
            //ISolver solver = new Day02Solver();
            //solver.Solve();
        }
    }
}