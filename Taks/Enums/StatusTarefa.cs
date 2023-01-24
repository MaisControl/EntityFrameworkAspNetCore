using System.ComponentModel;

namespace Taks.Enums
{
    public enum StatusTarefa
    {
        [Description("A Fazer")]
        Afazer= 1,
        [Description("Em Andamento")]
        EmAndamento = 2,
        [Description("Concluído")]
        Concluido = 3
    }
}
