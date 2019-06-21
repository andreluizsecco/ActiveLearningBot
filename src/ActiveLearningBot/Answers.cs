namespace ActiveLearningBot
{
    public static class Conversation
    {
        public static string Answer(string intentName)
        {
            string message = string.Empty;
            switch (intentName)
            {
                case "Saudacao":
                    message = "Olá! O que posso fazer por você?";
                    break;
                case "Reserva":
                    message = "Ótimo! Esses são os quartos que estão disponíveis...";
                    break;
                case "ServicoQuarto":
                    message = "Entendi, iremos enviar nosso colaborador, qual é o número do seu quarto?";
                    break;
                case "Despertador":
                    message = "Certo, pode contar comigo, não irei dormir. Que horário deseja ser acordado?";
                    break;
                default:
                    message = "Desculpe, não entendi o que você deseja...";
                    break;
            }

            return message;
        }
    }
}
