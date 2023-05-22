using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace crud.Models
{
    public class Clientes
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve ter 14 dígitos.")]
        [ValidarCNPJ(ErrorMessage = "CNPJ inválido.")]
        public string CNPJ { get; set; }

        public DateTime DataCadastro { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
    }

    public class ValidarCNPJAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string cnpj = (string)value;

            if (string.IsNullOrEmpty(cnpj))
                return ValidationResult.Success;

            // Remove caracteres não numéricos
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            // Verifica se o CNPJ possui 14 dígitos
            if (cnpj.Length != 14)
                return new ValidationResult(ErrorMessage);

            // Verifica se todos os dígitos são iguais, o que invalida o CNPJ
            if (new string(cnpj[0], 14) == cnpj)
                return new ValidationResult(ErrorMessage);

            var dbContext = validationContext.GetService(typeof(BancoDeDados)) as BancoDeDados;

            // Verifica se o CNPJ já existe no banco de dados
            bool cnpjExists = dbContext.Clientes.Any(c => c.CNPJ == cnpj && c.Id != (validationContext.ObjectInstance as Clientes).Id);
            if (cnpjExists)
                return new ValidationResult("CNPJ já cadastrado.");

            return ValidationResult.Success;
        }
    }

}
