namespace BackEnd.DTOs
{
    public class MensagemDto(string Mensagem)
    {
        public string Mensagem { get; set; } = Mensagem;
    }
}
