using System;
using System.Collections.Generic;

namespace DesignPatternBuilderSolution
{
    public class SalesReport
    {
        public string Title { get; private set; }
        public string Format { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        
        public bool IncludeHeader { get; private set; }
        public string HeaderText { get; private set; }
        public bool IncludeFooter { get; private set; }
        public string FooterText { get; private set; }
        
        public bool IncludeCharts { get; private set; }
        public string ChartType { get; private set; }
        public List<string> Columns { get; private set; } = new List<string>();
        public List<string> Filters { get; private set; } = new List<string>();
        
        public string Orientation { get; private set; }
        public string WaterMark { get; private set; }

        private SalesReport() { }

        public class Builder
        {
            private readonly SalesReport _report = new SalesReport();

            public Builder(string title)
            {
                _report.Title = title;
                _report.Format = "PDF"; 
                _report.StartDate = DateTime.Now;
                _report.EndDate = DateTime.Now;
                _report.Orientation = "Portrait";
            }
            
            public Builder SetFormat(string format)
            {
                _report.Format = format;
                return this;
            }

            public Builder SetPeriod(DateTime start, DateTime end)
            {
                _report.StartDate = start;
                _report.EndDate = end;
                return this;
            }

            public Builder WithHeader(string text)
            {
                _report.IncludeHeader = true;
                _report.HeaderText = text;
                return this;
            }

            public Builder WithFooter(string text)
            {
                _report.IncludeFooter = true;
                _report.FooterText = text;
                return this;
            }

            public Builder AddChart(string chartType)
            {
                _report.IncludeCharts = true;
                _report.ChartType = chartType;
                return this;
            }

            public Builder AddColumn(string column)
            {
                _report.Columns.Add(column);
                return this;
            }

            public Builder AddFilter(string filter)
            {
                _report.Filters.Add(filter);
                return this;
            }

            public Builder SetOrientation(string orientation)
            {
                _report.Orientation = orientation;
                return this;
            }
            
            public Builder WithWatermark(string text)
            {
                _report.WaterMark = text;
                return this;
            }

            public SalesReport Build()
            {
                if (_report.Columns.Count == 0)
                {
                    throw new InvalidOperationException("O relatório precisa de pelo menos uma coluna.");
                }

                return _report;
            }
        }

        public void Generate()
        {
            Console.WriteLine($"\n=== Gerando Relatório: {Title} ===");
            Console.WriteLine($"Formato: {Format} | Orientação: {Orientation}");
            Console.WriteLine($"Período: {StartDate:d} a {EndDate:d}");
            
            if (IncludeHeader) Console.WriteLine($"[CABEÇALHO]: {HeaderText}");
            if (!string.IsNullOrEmpty(WaterMark)) Console.WriteLine($"[MARCA D'ÁGUA]: {WaterMark}");
            
            Console.WriteLine($"Colunas: {string.Join(", ", Columns)}");
            
            if (IncludeCharts) Console.WriteLine($"[GRÁFICO]: Gerando gráfico de {ChartType}...");
            if (IncludeFooter) Console.WriteLine($"[RODAPÉ]: {FooterText}");
            
            Console.WriteLine(">> Relatório concluído.");
        }
    }

    public class ReportDirector
    {
        public static SalesReport BuildMonthlySales(SalesReport.Builder builder)
        {
            return builder
                .SetFormat("PDF")
                .WithHeader("Relatório Mensal de Vendas")
                .AddColumn("Produto")
                .AddColumn("Quantidade")
                .AddColumn("Total R$")
                .AddChart("Barra")
                .WithFooter("Uso Interno - Confidencial")
                .Build();
        }

        public static SalesReport BuildSimpleList(SalesReport.Builder builder)
        {
            return builder
                .SetFormat("Excel")
                .AddColumn("Nome")
                .AddColumn("Email")
                .SetOrientation("Landscape")
                .Build();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Builder Pattern em Ação ===\n");

            var report1 = new SalesReport.Builder("Relatório Ad-Hoc da Diretoria")
                .SetPeriod(new DateTime(2024, 1, 1), new DateTime(2024, 6, 30))
                .WithHeader("Análise Semestral")
                .WithWatermark("CONFIDENCIAL")
                .AddColumn("Departamento")
                .AddColumn("Lucro Líquido")
                .AddChart("Pizza")
                .SetOrientation("Landscape")
                .Build();

            report1.Generate();

            var builderMensal = new SalesReport.Builder("Vendas Agosto/2024");
            var report2 = ReportDirector.BuildMonthlySales(builderMensal);

            report2.Generate();

            try 
            {
                var reportIncompleto = new SalesReport.Builder("Relatório Quebrado")
                    .SetFormat("HTML")
                    .Build(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERRO]: Falha ao criar relatório: {ex.Message}");
            }
        }
    }
}
