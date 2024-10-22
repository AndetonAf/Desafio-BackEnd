using static BackEnd.Services.RentalService;

namespace BackEnd.DTOs.LocacaoController
{
    public class RentalPutResponseDto(string mensagem, RentalValue values)
    {
        public string Mensagem { get; set; } = mensagem;
        public RentalValue Values { get; set; } = values;
    }
}
