using System;
using BusinessLogicLayer;

namespace PT_Readify
{
    public static class CarteiraService
    {
        private static decimal _saldo;
        private static int _utilizadorCarregado = -1;

        public static event Action SaldoAlterado;

        public static decimal Saldo
        {
            get
            {
                GarantirCarregado();
                return _saldo;
            }
        }

        public static void CarregarParaUtilizador(int idUtilizador)
        {
            if (idUtilizador <= 0)
            {
                Limpar();
                return;
            }

            if (_utilizadorCarregado == idUtilizador)
                return;

            _utilizadorCarregado = idUtilizador;
            _saldo = BLL.Carteira.ObterSaldo(idUtilizador);
            SaldoAlterado?.Invoke();
        }

        public static void Limpar()
        {
            _utilizadorCarregado = -1;
            _saldo = 0m;
            SaldoAlterado?.Invoke();
        }

        private static void GarantirCarregado()
        {
            if (_utilizadorCarregado <= 0 && globais.id_utilizador > 0)
                CarregarParaUtilizador(globais.id_utilizador);
        }

        private static void GarantirUtilizadorAutenticado()
        {
            GarantirCarregado();

            if (_utilizadorCarregado <= 0)
                throw new InvalidOperationException("Inicie sessão para usar a carteira.");
        }

        private static void PersistirSaldo()
        {
            BLL.Carteira.AtualizarSaldo(_utilizadorCarregado, _saldo);
        }

        public static bool TemSaldoSuficiente(decimal valor)
        {
            GarantirCarregado();
            return _utilizadorCarregado > 0 && _saldo >= valor;
        }

        public static void Debitar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentOutOfRangeException(nameof(valor), "O valor a debitar deve ser positivo.");

            GarantirUtilizadorAutenticado();

            if (_saldo < valor)
                throw new InvalidOperationException(
                    $"Saldo insuficiente. Saldo disponível: {_saldo:C2}. Valor necessário: {valor:C2}.");

            _saldo -= valor;
            PersistirSaldo();
            SaldoAlterado?.Invoke();
        }

        public static void AdicionarSaldo(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentOutOfRangeException(nameof(valor), "O valor a adicionar deve ser positivo.");

            GarantirUtilizadorAutenticado();

            _saldo += valor;
            PersistirSaldo();
            SaldoAlterado?.Invoke();
        }
    }
}
