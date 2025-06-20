using MessengerAPI.Dto;
using MessengerAPI.Models;
using MessengerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace MessengerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : BaseController
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto createChatDto)
    {
        try
        {
            var chat = await _chatService.CreateChat(createChatDto);
            return CreatedAtAction(nameof(CreateChat), new { id = chat.Id }, chat);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("link")]
    public async Task<IActionResult> GetInvitationLink(int id)
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var link = await _chatService.GetInvitationLink(id, baseUrl);
            return Ok(new { Link = link });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinChat([FromQuery] Guid guid)
    {
        try
        {
            if (guid == Guid.Empty)
            {
                return BadRequest("Invalid chat link.");
            }

            var userId = GetUserId();
            var result = await _chatService.JoinChatByLink(guid, userId);
            return CreatedAtAction(nameof(JoinChat), new { chatId = result.Id_chat }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Получение списка групповых чатов
    /// </summary>
    /// <returns>Список групповых чатов у пользователя</returns>

    [HttpGet("chats")]
    public async Task<IActionResult> GetChat()
    {
        try
        {
            var userId = GetUserId();
            var result = await _chatService.GetChatsByUser(userId);
            if (result == null)
                return BadRequest("У пользователя нет групповых чатов");
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Создание или открытие личного чата
    /// </summary>
    /// <param name="idChat">Id чата</param>
    /// <returns>Данные созданного чата</returns>

    [HttpPost("personal")]
    public async Task<IActionResult> CreatePersonalChat([FromBody] CreatePersonalChatDto createPersonalChatDto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _chatService.CreatePersonalChat(createPersonalChatDto, userId);
            if (result == null)
            {
                return StatusCode(500, "Не удалось создать чат.");
            }
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Добавление пользователя в групповой чат
    /// </summary>
    /// <param name="addUserInChatDto">Входные данные</param>
    /// <returns>IActionResult</returns>

    [HttpPost("add_user")]
    public async Task<IActionResult> AddUserInChat([FromBody] AddUserInChatDto addUserInChatDto)
    {
        try
        {
            bool result = await _chatService.AddUserInChat(addUserInChatDto);

            if (result)
            {
                return Ok("User has been successfully added to the group chat");
            }
            else 
            {
                return Conflict("User is already in the group chat.");
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
