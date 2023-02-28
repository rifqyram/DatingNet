using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enigma.DatingNet.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_interest",
                columns: table => new
                {
                    interestid = table.Column<Guid>(name: "interest_id", type: "uuid", nullable: false),
                    interest = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_interest", x => x.interestid);
                });

            migrationBuilder.CreateTable(
                name: "m_member_user_access",
                columns: table => new
                {
                    memberid = table.Column<Guid>(name: "member_id", type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    joindate = table.Column<DateTime>(name: "join_date", type: "timestamp with time zone", nullable: false),
                    verificationstatus = table.Column<string>(name: "verification_status", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_member_user_access", x => x.memberid);
                });

            migrationBuilder.CreateTable(
                name: "m_member_contact_info",
                columns: table => new
                {
                    membercontactid = table.Column<Guid>(name: "member_contact_id", type: "uuid", nullable: false),
                    mobilephonenumber = table.Column<string>(name: "mobile_phone_number", type: "text", nullable: false),
                    instagramid = table.Column<string>(name: "instagram_id", type: "text", nullable: true),
                    twitterid = table.Column<string>(name: "twitter_id", type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    memberid = table.Column<Guid>(name: "member_id", type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_member_contact_info", x => x.membercontactid);
                    table.ForeignKey(
                        name: "FK_m_member_contact_info_m_member_user_access_member_id",
                        column: x => x.memberid,
                        principalTable: "m_member_user_access",
                        principalColumn: "member_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "m_member_interest",
                columns: table => new
                {
                    memberinterestid = table.Column<Guid>(name: "member_interest_id", type: "uuid", nullable: false),
                    interestid = table.Column<Guid>(name: "interest_id", type: "uuid", nullable: false),
                    memberid = table.Column<Guid>(name: "member_id", type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_member_interest", x => x.memberinterestid);
                    table.ForeignKey(
                        name: "FK_m_member_interest_m_interest_interest_id",
                        column: x => x.interestid,
                        principalTable: "m_interest",
                        principalColumn: "interest_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_m_member_interest_m_member_user_access_member_id",
                        column: x => x.memberid,
                        principalTable: "m_member_user_access",
                        principalColumn: "member_id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "m_member_personal_info",
                columns: table => new
                {
                    personalinfoid = table.Column<Guid>(name: "personal_info_id", type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    bod = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "text", nullable: false),
                    selfdescription = table.Column<string>(name: "self_description", type: "text", nullable: false),
                    recentphotopath = table.Column<string>(name: "recent_photo_path", type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    memberid = table.Column<Guid>(name: "member_id", type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_member_personal_info", x => x.personalinfoid);
                    table.ForeignKey(
                        name: "FK_m_member_personal_info_m_member_user_access_member_id",
                        column: x => x.memberid,
                        principalTable: "m_member_user_access",
                        principalColumn: "member_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "m_member_preferences",
                columns: table => new
                {
                    preferenceid = table.Column<Guid>(name: "preference_id", type: "uuid", nullable: false),
                    lookingforgender = table.Column<string>(name: "looking_for_gender", type: "text", nullable: false),
                    lookingfordomicile = table.Column<string>(name: "looking_for_domicile", type: "text", nullable: false),
                    lookingforstartage = table.Column<int>(name: "looking_for_start_age", type: "integer", nullable: false),
                    lookingforendage = table.Column<int>(name: "looking_for_end_age", type: "integer", nullable: false),
                    memberid = table.Column<Guid>(name: "member_id", type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_member_preferences", x => x.preferenceid);
                    table.ForeignKey(
                        name: "FK_m_member_preferences_m_member_user_access_member_id",
                        column: x => x.memberid,
                        principalTable: "m_member_user_access",
                        principalColumn: "member_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_m_member_contact_info_member_id",
                table: "m_member_contact_info",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_member_interest_interest_id",
                table: "m_member_interest",
                column: "interest_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_member_interest_member_id",
                table: "m_member_interest",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_member_partner_member_id",
                table: "m_member_partner",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_member_partner_partner_id",
                table: "m_member_partner",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_member_personal_info_member_id",
                table: "m_member_personal_info",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_member_preferences_member_id",
                table: "m_member_preferences",
                column: "member_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_member_contact_info");

            migrationBuilder.DropTable(
                name: "m_member_interest");

            migrationBuilder.DropTable(
                name: "m_member_partner");

            migrationBuilder.DropTable(
                name: "m_member_personal_info");

            migrationBuilder.DropTable(
                name: "m_member_preferences");

            migrationBuilder.DropTable(
                name: "m_interest");

            migrationBuilder.DropTable(
                name: "m_member_user_access");
        }
    }
}
