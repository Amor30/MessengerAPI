using MessengerAPI.Models;

namespace MessengerAPI.Dto
{
    public class AddUserInChatDto
    {
        public int id_chat { get; set; }
        public int id_user { get; set; }
    }
}
