using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enigma.DatingNet.Migrations
{
    /// <inheritdoc />
    public partial class PartnerCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_member_partner",
                columns: table => new
                {
                    memberpartnerid = table.Column<Guid>(name: "member_partner_id", type: "uuid", nullable: false),
                    memberid = table.Column<Guid>(name: "member_id", type: "uuid", nullable: false),
                    partnerid = table.Column<Guid>(name: "partner_id", type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_member_partner", x => x.memberpartnerid);
                    table.ForeignKey(
                        name: "FK_m_member_partner_m_member_user_access_member_id",
                        column: x => x.memberid,
                        principalTable: "m_member_user_access",
                        principalColumn: "member_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_m_member_partner_m_member_user_access_partner_id",
                        column: x => x.partnerid,
                        principalTable: "m_member_user_access",
                        principalColumn: "member_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_m_member_partner_member_id",
                table: "m_member_partner",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_member_partner_partner_id",
                table: "m_member_partner",
                column: "partner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_member_partner");
        }
    }
}
