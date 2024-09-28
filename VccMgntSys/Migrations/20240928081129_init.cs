using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VccMgntSys.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    DateofBirth = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    JobDescription = table.Column<string>(type: "text", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "boolean", nullable: true),
                    StringCitizenID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "citizens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CitizenID = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    VaccinationCount = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<string>(type: "text", nullable: false),
                    VaccinationDate = table.Column<string>(type: "text", nullable: true),
                    ReportData = table.Column<string>(type: "text", nullable: true),
                    OtherDiseases = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Pending = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citizens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DateofBirth = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    JobDescription = table.Column<string>(type: "text", nullable: false),
                    HospitalID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DateofBirth = table.Column<string>(type: "text", nullable: false),
                    HospitalId = table.Column<string>(type: "text", nullable: false),
                    JobDescription = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    CitizenId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Datadate = table.Column<string>(type: "text", nullable: false),
                    VaccinatedCitizens = table.Column<long>(type: "bigint", nullable: false),
                    TotalVaccinations = table.Column<long>(type: "bigint", nullable: false),
                    TotalPrograms = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vaccineBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    ExpirationDate = table.Column<string>(type: "text", nullable: false),
                    ProducedDate = table.Column<string>(type: "text", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    BatchId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vaccineBatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vaccinePrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vaccinePrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vaccinePrograms_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Manager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CitizenVaccineProgram",
                columns: table => new
                {
                    CitizensId = table.Column<Guid>(type: "uuid", nullable: false),
                    VaccineProgramId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenVaccineProgram", x => new { x.CitizensId, x.VaccineProgramId });
                    table.ForeignKey(
                        name: "FK_CitizenVaccineProgram_citizens_CitizensId",
                        column: x => x.CitizensId,
                        principalTable: "citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CitizenVaccineProgram_vaccinePrograms_VaccineProgramId",
                        column: x => x.VaccineProgramId,
                        principalTable: "vaccinePrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageDate = table.Column<string>(type: "text", nullable: false),
                    CitizenId = table.Column<Guid>(type: "uuid", nullable: false),
                    VaccineProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    Confirm = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_messages_citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messages_vaccinePrograms_VaccineProgramId",
                        column: x => x.VaccineProgramId,
                        principalTable: "vaccinePrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffVaccineProgram",
                columns: table => new
                {
                    StaffsId = table.Column<Guid>(type: "uuid", nullable: false),
                    VaccineProgramsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffVaccineProgram", x => new { x.StaffsId, x.VaccineProgramsId });
                    table.ForeignKey(
                        name: "FK_StaffVaccineProgram_Staff_StaffsId",
                        column: x => x.StaffsId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffVaccineProgram_vaccinePrograms_VaccineProgramsId",
                        column: x => x.VaccineProgramsId,
                        principalTable: "vaccinePrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaccineBatchVaccineProgram",
                columns: table => new
                {
                    VaccineBatchesId = table.Column<Guid>(type: "uuid", nullable: false),
                    VaccineProgramsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineBatchVaccineProgram", x => new { x.VaccineBatchesId, x.VaccineProgramsId });
                    table.ForeignKey(
                        name: "FK_VaccineBatchVaccineProgram_vaccineBatches_VaccineBatchesId",
                        column: x => x.VaccineBatchesId,
                        principalTable: "vaccineBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaccineBatchVaccineProgram_vaccinePrograms_VaccineProgramsId",
                        column: x => x.VaccineProgramsId,
                        principalTable: "vaccinePrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CitizenVaccineProgram_VaccineProgramId",
                table: "CitizenVaccineProgram",
                column: "VaccineProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_CitizenId",
                table: "messages",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_VaccineProgramId",
                table: "messages",
                column: "VaccineProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffVaccineProgram_VaccineProgramsId",
                table: "StaffVaccineProgram",
                column: "VaccineProgramsId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineBatchVaccineProgram_VaccineProgramsId",
                table: "VaccineBatchVaccineProgram",
                column: "VaccineProgramsId");

            migrationBuilder.CreateIndex(
                name: "IX_vaccinePrograms_ManagerId",
                table: "vaccinePrograms",
                column: "ManagerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "CitizenVaccineProgram");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "StaffVaccineProgram");

            migrationBuilder.DropTable(
                name: "statistics");

            migrationBuilder.DropTable(
                name: "VaccineBatchVaccineProgram");

            migrationBuilder.DropTable(
                name: "citizens");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "vaccineBatches");

            migrationBuilder.DropTable(
                name: "vaccinePrograms");

            migrationBuilder.DropTable(
                name: "Manager");
        }
    }
}
