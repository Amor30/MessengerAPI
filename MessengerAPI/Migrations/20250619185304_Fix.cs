using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerAPI.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_TypeChats_Id_type_chat",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_Id_user",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_Id_chat",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChats_AspNetUsers_Id_user",
                table: "UserChats");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChats_Chats_Id_chat",
                table: "UserChats");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChats",
                table: "UserChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeChats",
                table: "TypeChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "UserChats",
                newName: "tb_user_chat");

            migrationBuilder.RenameTable(
                name: "TypeChats",
                newName: "tb_type_chat");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "tb_message");

            migrationBuilder.RenameTable(
                name: "Chats",
                newName: "tb_chat");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "AspNetUsers",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AspNetUsers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Create_date",
                table: "AspNetUsers",
                newName: "create_date");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AspNetUsers",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "AspNetUsers",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Id_chat",
                table: "tb_user_chat",
                newName: "id_chat");

            migrationBuilder.RenameColumn(
                name: "Id_user",
                table: "tb_user_chat",
                newName: "id_user");

            migrationBuilder.RenameIndex(
                name: "IX_UserChats_Id_chat",
                table: "tb_user_chat",
                newName: "IX_tb_user_chat_id_chat");

            migrationBuilder.RenameColumn(
                name: "Name_type",
                table: "tb_type_chat",
                newName: "name_type");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tb_type_chat",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "User_name",
                table: "tb_message",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "Id_user",
                table: "tb_message",
                newName: "id_user");

            migrationBuilder.RenameColumn(
                name: "Id_chat",
                table: "tb_message",
                newName: "id_chat");

            migrationBuilder.RenameColumn(
                name: "Create_date",
                table: "tb_message",
                newName: "create_date");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tb_message",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_Id_user",
                table: "tb_message",
                newName: "IX_tb_message_id_user");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_Id_chat",
                table: "tb_message",
                newName: "IX_tb_message_id_chat");

            migrationBuilder.RenameColumn(
                name: "Id_type_chat",
                table: "tb_chat",
                newName: "id_type_chat");

            migrationBuilder.RenameColumn(
                name: "Create_date",
                table: "tb_chat",
                newName: "create_date");

            migrationBuilder.RenameColumn(
                name: "Chat_name",
                table: "tb_chat",
                newName: "chat_name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tb_chat",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "InvitationGuid",
                table: "tb_chat",
                newName: "guid");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_Id_type_chat",
                table: "tb_chat",
                newName: "IX_tb_chat_id_type_chat");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_user_chat",
                table: "tb_user_chat",
                columns: new[] { "id_user", "id_chat" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_type_chat",
                table: "tb_type_chat",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_message",
                table: "tb_message",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_chat",
                table: "tb_chat",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_email",
                table: "AspNetUsers",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_chat_tb_type_chat_id_type_chat",
                table: "tb_chat",
                column: "id_type_chat",
                principalTable: "tb_type_chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_message_AspNetUsers_id_user",
                table: "tb_message",
                column: "id_user",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_message_tb_chat_id_chat",
                table: "tb_message",
                column: "id_chat",
                principalTable: "tb_chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_user_chat_AspNetUsers_id_user",
                table: "tb_user_chat",
                column: "id_user",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_user_chat_tb_chat_id_chat",
                table: "tb_user_chat",
                column: "id_chat",
                principalTable: "tb_chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_chat_tb_type_chat_id_type_chat",
                table: "tb_chat");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_message_AspNetUsers_id_user",
                table: "tb_message");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_message_tb_chat_id_chat",
                table: "tb_message");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_user_chat_AspNetUsers_id_user",
                table: "tb_user_chat");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_user_chat_tb_chat_id_chat",
                table: "tb_user_chat");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_email",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_user_chat",
                table: "tb_user_chat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_type_chat",
                table: "tb_type_chat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_message",
                table: "tb_message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_chat",
                table: "tb_chat");

            migrationBuilder.RenameTable(
                name: "tb_user_chat",
                newName: "UserChats");

            migrationBuilder.RenameTable(
                name: "tb_type_chat",
                newName: "TypeChats");

            migrationBuilder.RenameTable(
                name: "tb_message",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "tb_chat",
                newName: "Chats");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "AspNetUsers",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AspNetUsers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "AspNetUsers",
                newName: "Create_date");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "AspNetUsers",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "id_chat",
                table: "UserChats",
                newName: "Id_chat");

            migrationBuilder.RenameColumn(
                name: "id_user",
                table: "UserChats",
                newName: "Id_user");

            migrationBuilder.RenameIndex(
                name: "IX_tb_user_chat_id_chat",
                table: "UserChats",
                newName: "IX_UserChats_Id_chat");

            migrationBuilder.RenameColumn(
                name: "name_type",
                table: "TypeChats",
                newName: "Name_type");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "TypeChats",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "Messages",
                newName: "User_name");

            migrationBuilder.RenameColumn(
                name: "id_user",
                table: "Messages",
                newName: "Id_user");

            migrationBuilder.RenameColumn(
                name: "id_chat",
                table: "Messages",
                newName: "Id_chat");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "Messages",
                newName: "Create_date");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Messages",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_tb_message_id_user",
                table: "Messages",
                newName: "IX_Messages_Id_user");

            migrationBuilder.RenameIndex(
                name: "IX_tb_message_id_chat",
                table: "Messages",
                newName: "IX_Messages_Id_chat");

            migrationBuilder.RenameColumn(
                name: "id_type_chat",
                table: "Chats",
                newName: "Id_type_chat");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "Chats",
                newName: "Create_date");

            migrationBuilder.RenameColumn(
                name: "chat_name",
                table: "Chats",
                newName: "Chat_name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Chats",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "Chats",
                newName: "InvitationGuid");

            migrationBuilder.RenameIndex(
                name: "IX_tb_chat_id_type_chat",
                table: "Chats",
                newName: "IX_Chats_Id_type_chat");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChats",
                table: "UserChats",
                columns: new[] { "Id_user", "Id_chat" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeChats",
                table: "TypeChats",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_TypeChats_Id_type_chat",
                table: "Chats",
                column: "Id_type_chat",
                principalTable: "TypeChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_Id_user",
                table: "Messages",
                column: "Id_user",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_Id_chat",
                table: "Messages",
                column: "Id_chat",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChats_AspNetUsers_Id_user",
                table: "UserChats",
                column: "Id_user",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChats_Chats_Id_chat",
                table: "UserChats",
                column: "Id_chat",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
