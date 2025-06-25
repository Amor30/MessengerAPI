using MessengerAPI.Models;
using MessengerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MessengerAPI.Dto;

public class CreatePersonalChatDto
{
    public int User_id { get; set; }
    public string? Chat_name { get; set; }
}
