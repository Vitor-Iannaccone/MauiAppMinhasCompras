using SQLite;

namespace MauiAppMinhasCompras.Models
{

    public enum CategoriaTipo
    {
        Alimentos,
        Higiene,
        Limpeza,
        Outros
    }
    public class Produtos
    {
        string _descricao;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao
        {
            get => _descricao;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a descrição.");
                }
                _descricao = value;
            }
        }
        public double Quantidade { get; set; }
        public double Preco { get; set; }

        public double Total { get => Quantidade * Preco; }

        public CategoriaTipo CategoriaTipo { get; set; }

        public string CategoriaSigla
        {
            get
            {
                switch (CategoriaTipo)
                {
                    case CategoriaTipo.Alimentos:
                        return "A";
                    case CategoriaTipo.Higiene:
                        return "H";
                    case CategoriaTipo.Limpeza:
                        return "L";
                    default:
                        return "O";
                }
            }
        }
    }
}
