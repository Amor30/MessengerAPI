namespace MessengerAPI.Dto;

public class ChatDto
{
    public int Id { get; set; }
    public string Chat_name { get; set; }
    public DateTime Create_date { get; set; }
    public int Id_type_chat { get; set; }
    public Guid InvitationGuid { get; set; }
}