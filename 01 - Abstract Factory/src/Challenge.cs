using System;

namespace DesignPatternSolution
{
    public interface IPaymentValidator
    {
        bool ValidateCard(string cardNumber);
    }

    public interface IPaymentProcessor
    {
        string ProcessTransaction(decimal amount, string cardNumber);
    }

    public interface IPaymentLogger
    {
        void Log(string message);
    }

    public interface IPaymentGatewayFactory
    {
        IPaymentValidator CreateValidator();
        IPaymentProcessor CreateProcessor();
        IPaymentLogger CreateLogger();
    }

    public class PagSeguroValidator : IPaymentValidator {
        public bool ValidateCard(string cardNumber) {
            Console.WriteLine("PagSeguro: Validando cartão...");
            return cardNumber.Length == 16;
        }
    }
    public class PagSeguroProcessor : IPaymentProcessor {
        public string ProcessTransaction(decimal amount, string cardNumber) {
            Console.WriteLine($"PagSeguro: Processando R$ {amount}...");
            return $"PAGSEG-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
    public class PagSeguroLogger : IPaymentLogger {
        public void Log(string message) => Console.WriteLine($"[PagSeguro Log] {DateTime.Now}: {message}");
    }

    public class MercadoPagoValidator : IPaymentValidator {
        public bool ValidateCard(string cardNumber) {
            Console.WriteLine("MercadoPago: Validando cartão...");
            return cardNumber.Length == 16 && cardNumber.StartsWith("5");
        }
    }
    public class MercadoPagoProcessor : IPaymentProcessor {
        public string ProcessTransaction(decimal amount, string cardNumber) {
            Console.WriteLine($"MercadoPago: Processando R$ {amount}...");
            return $"MP-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
    public class MercadoPagoLogger : IPaymentLogger {
        public void Log(string message) => Console.WriteLine($"[MercadoPago Log] {DateTime.Now}: {message}");
    }

    public class PagSeguroFactory : IPaymentGatewayFactory
    {
        public IPaymentValidator CreateValidator() => new PagSeguroValidator();
        public IPaymentProcessor CreateProcessor() => new PagSeguroProcessor();
        public IPaymentLogger CreateLogger() => new PagSeguroLogger();
    }

    public class MercadoPagoFactory : IPaymentGatewayFactory
    {
        public IPaymentValidator CreateValidator() => new MercadoPagoValidator();
        public IPaymentProcessor CreateProcessor() => new MercadoPagoProcessor();
        public IPaymentLogger CreateLogger() => new MercadoPagoLogger();
    }

    public class PaymentService
    {
        private readonly IPaymentGatewayFactory _factory;

        public PaymentService(IPaymentGatewayFactory factory)
        {
            _factory = factory;
        }

        public void ProcessPayment(decimal amount, string cardNumber)
        {
            var validator = _factory.CreateValidator();
            var processor = _factory.CreateProcessor();
            var logger = _factory.CreateLogger();

            if (!validator.ValidateCard(cardNumber))
            {
                logger.Log("Falha na validação do cartão.");
                return;
            }

            var result = processor.ProcessTransaction(amount, cardNumber);
            logger.Log($"Transação processada com sucesso: {result}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Pagamentos Refatorado ===\n");

            IPaymentGatewayFactory factoryAtual = new PagSeguroFactory();
            
            var servicoDePagamento = new PaymentService(factoryAtual);
            servicoDePagamento.ProcessPayment(150.00m, "1234567890123456");

            Console.WriteLine("\n--- Trocando Gateway dinamicamente ---\n");

            factoryAtual = new MercadoPagoFactory();
            
            servicoDePagamento = new PaymentService(factoryAtual);
            servicoDePagamento.ProcessPayment(200.00m, "5234567890123456");
        }
    }
}
