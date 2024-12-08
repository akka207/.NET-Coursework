using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PM.API.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Schedule_ScheduleId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Person_PersonId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Role_RoleId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Schedule_ScheduleId",
                table: "Staff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Staff",
                table: "Staff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "Event");

            migrationBuilder.RenameTable(
                name: "Staff",
                newName: "Staffs");

            migrationBuilder.RenameTable(
                name: "Schedule",
                newName: "Schedules");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Persons");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_Staff_ScheduleId",
                table: "Staffs",
                newName: "IX_Staffs_ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Staff_RoleId",
                table: "Staffs",
                newName: "IX_Staffs_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Staff_PersonId",
                table: "Staffs",
                newName: "IX_Staffs_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Event_ScheduleId",
                table: "Events",
                newName: "IX_Events_ScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Staffs",
                table: "Staffs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Schedules_ScheduleId",
                table: "Events",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Persons_PersonId",
                table: "Staffs",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Roles_RoleId",
                table: "Staffs",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Schedules_ScheduleId",
                table: "Staffs",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Schedules_ScheduleId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Persons_PersonId",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Roles_RoleId",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Schedules_ScheduleId",
                table: "Staffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Staffs",
                table: "Staffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Staffs",
                newName: "Staff");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "Schedule");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "Person");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameIndex(
                name: "IX_Staffs_ScheduleId",
                table: "Staff",
                newName: "IX_Staff_ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Staffs_RoleId",
                table: "Staff",
                newName: "IX_Staff_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Staffs_PersonId",
                table: "Staff",
                newName: "IX_Staff_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_ScheduleId",
                table: "Event",
                newName: "IX_Event_ScheduleId");

            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "Event",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Staff",
                table: "Staff",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Schedule_ScheduleId",
                table: "Event",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Person_PersonId",
                table: "Staff",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Role_RoleId",
                table: "Staff",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Schedule_ScheduleId",
                table: "Staff",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
