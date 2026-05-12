using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Transit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dzielnice",
                columns: table => new
                {
                    id_dzielnicy = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dzielnice", x => x.id_dzielnicy);
                });

            migrationBuilder.CreateTable(
                name: "Kategorie_Biletow",
                columns: table => new
                {
                    id_kategorii = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    znizka_pct = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorie_Biletow", x => x.id_kategorii);
                });

            migrationBuilder.CreateTable(
                name: "Kierowcy",
                columns: table => new
                {
                    id_kierowcy = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    imie = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nazwisko = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nr_pracownika = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    data_zatrudnienia = table.Column<DateOnly>(type: "date", nullable: false),
                    aktywny = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kierowcy", x => x.id_kierowcy);
                });

            migrationBuilder.CreateTable(
                name: "Kontrolerzy",
                columns: table => new
                {
                    id_kontrolera = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    imie = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nazwisko = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nr_sluzbowy = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    aktywny = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kontrolerzy", x => x.id_kontrolera);
                });

            migrationBuilder.CreateTable(
                name: "Linie",
                columns: table => new
                {
                    id_linii = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numer_linii = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    typ_linii = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    opis = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    aktywna = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Linie", x => x.id_linii);
                });

            migrationBuilder.CreateTable(
                name: "Pasazerowie",
                columns: table => new
                {
                    id_pasazera = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    imie = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nazwisko = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    pesel = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasazerowie", x => x.id_pasazera);
                });

            migrationBuilder.CreateTable(
                name: "Producenci_Pojazdow",
                columns: table => new
                {
                    id_producenta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producenci_Pojazdow", x => x.id_producenta);
                });

            migrationBuilder.CreateTable(
                name: "Typy_Dni",
                columns: table => new
                {
                    id_typu_dnia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kod = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    nazwa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Typy_Dni", x => x.id_typu_dnia);
                });

            migrationBuilder.CreateTable(
                name: "Typy_Przegladu",
                columns: table => new
                {
                    id_typu_przegladu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kod = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    nazwa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    interwal_dni = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Typy_Przegladu", x => x.id_typu_przegladu);
                });

            migrationBuilder.CreateTable(
                name: "Uprawnienia_Kategorii",
                columns: table => new
                {
                    id_uprawnienia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategoria = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    opis = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uprawnienia_Kategorii", x => x.id_uprawnienia);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Przystanki",
                columns: table => new
                {
                    id_przystanku = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_dzielnicy = table.Column<int>(type: "integer", nullable: false),
                    nazwa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ulica = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    typ = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "naziemny"),
                    wiata = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    aktywny = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Przystanki", x => x.id_przystanku);
                    table.ForeignKey(
                        name: "FK_Przystanki_Dzielnice_id_dzielnicy",
                        column: x => x.id_dzielnicy,
                        principalTable: "Dzielnice",
                        principalColumn: "id_dzielnicy",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Badania_Lekarskie",
                columns: table => new
                {
                    id_badania = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_kierowcy = table.Column<int>(type: "integer", nullable: false),
                    data_badania = table.Column<DateOnly>(type: "date", nullable: false),
                    data_waznosci = table.Column<DateOnly>(type: "date", nullable: false),
                    wynik = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    lekarz = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badania_Lekarskie", x => x.id_badania);
                    table.ForeignKey(
                        name: "FK_Badania_Lekarskie_Kierowcy_id_kierowcy",
                        column: x => x.id_kierowcy,
                        principalTable: "Kierowcy",
                        principalColumn: "id_kierowcy",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Warianty_Trasy",
                columns: table => new
                {
                    id_wariantu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_linii = table.Column<int>(type: "integer", nullable: false),
                    nazwa_wariantu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    kierunek = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    aktywny = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warianty_Trasy", x => x.id_wariantu);
                    table.ForeignKey(
                        name: "FK_Warianty_Trasy_Linie_id_linii",
                        column: x => x.id_linii,
                        principalTable: "Linie",
                        principalColumn: "id_linii",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bilety_Okresowe",
                columns: table => new
                {
                    id_biletu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_pasazera = table.Column<int>(type: "integer", nullable: false),
                    id_kategorii = table.Column<int>(type: "integer", nullable: false),
                    data_od = table.Column<DateOnly>(type: "date", nullable: false),
                    data_do = table.Column<DateOnly>(type: "date", nullable: false),
                    cena = table.Column<decimal>(type: "numeric(8,2)", nullable: false),
                    aktywny = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bilety_Okresowe", x => x.id_biletu);
                    table.ForeignKey(
                        name: "FK_Bilety_Okresowe_Kategorie_Biletow_id_kategorii",
                        column: x => x.id_kategorii,
                        principalTable: "Kategorie_Biletow",
                        principalColumn: "id_kategorii",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bilety_Okresowe_Pasazerowie_id_pasazera",
                        column: x => x.id_pasazera,
                        principalTable: "Pasazerowie",
                        principalColumn: "id_pasazera",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modele_Pojazdow",
                columns: table => new
                {
                    id_modelu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_producenta = table.Column<int>(type: "integer", nullable: false),
                    nazwa_modelu = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    typ_pojazdu = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    liczba_miejsc = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modele_Pojazdow", x => x.id_modelu);
                    table.ForeignKey(
                        name: "FK_Modele_Pojazdow_Producenci_Pojazdow_id_producenta",
                        column: x => x.id_producenta,
                        principalTable: "Producenci_Pojazdow",
                        principalColumn: "id_producenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Uprawnienia_Kierowcow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_kierowcy = table.Column<int>(type: "integer", nullable: false),
                    id_uprawnienia = table.Column<int>(type: "integer", nullable: false),
                    data_uzyskania = table.Column<DateOnly>(type: "date", nullable: false),
                    data_waznosci = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uprawnienia_Kierowcow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uprawnienia_Kierowcow_Kierowcy_id_kierowcy",
                        column: x => x.id_kierowcy,
                        principalTable: "Kierowcy",
                        principalColumn: "id_kierowcy",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Uprawnienia_Kierowcow_Uprawnienia_Kategorii_id_uprawnienia",
                        column: x => x.id_uprawnienia,
                        principalTable: "Uprawnienia_Kategorii",
                        principalColumn: "id_uprawnienia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Przystanki_Wariantu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_wariantu = table.Column<int>(type: "integer", nullable: false),
                    id_przystanku = table.Column<int>(type: "integer", nullable: false),
                    kolejnosc = table.Column<int>(type: "integer", nullable: false),
                    odleglosc_od_poczatku_m = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Przystanki_Wariantu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Przystanki_Wariantu_Przystanki_id_przystanku",
                        column: x => x.id_przystanku,
                        principalTable: "Przystanki",
                        principalColumn: "id_przystanku",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Przystanki_Wariantu_Warianty_Trasy_id_wariantu",
                        column: x => x.id_wariantu,
                        principalTable: "Warianty_Trasy",
                        principalColumn: "id_wariantu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rozklady_Jazdy",
                columns: table => new
                {
                    id_rozkladu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_wariantu = table.Column<int>(type: "integer", nullable: false),
                    id_typu_dnia = table.Column<int>(type: "integer", nullable: false),
                    data_waznosci_od = table.Column<DateOnly>(type: "date", nullable: false),
                    data_waznosci_do = table.Column<DateOnly>(type: "date", nullable: true),
                    aktywny = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rozklady_Jazdy", x => x.id_rozkladu);
                    table.ForeignKey(
                        name: "FK_Rozklady_Jazdy_Typy_Dni_id_typu_dnia",
                        column: x => x.id_typu_dnia,
                        principalTable: "Typy_Dni",
                        principalColumn: "id_typu_dnia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rozklady_Jazdy_Warianty_Trasy_id_wariantu",
                        column: x => x.id_wariantu,
                        principalTable: "Warianty_Trasy",
                        principalColumn: "id_wariantu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pojazdy",
                columns: table => new
                {
                    id_pojazdu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_modelu = table.Column<int>(type: "integer", nullable: false),
                    numer_boczny = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    vin = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    rok_produkcji = table.Column<int>(type: "integer", nullable: false),
                    data_zakupu = table.Column<DateOnly>(type: "date", nullable: false),
                    aktywny = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pojazdy", x => x.id_pojazdu);
                    table.ForeignKey(
                        name: "FK_Pojazdy_Modele_Pojazdow_id_modelu",
                        column: x => x.id_modelu,
                        principalTable: "Modele_Pojazdow",
                        principalColumn: "id_modelu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Odjazdy_Planowe",
                columns: table => new
                {
                    id_odjazdu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_rozkladu = table.Column<int>(type: "integer", nullable: false),
                    id_przystanku_wariantu = table.Column<int>(type: "integer", nullable: false),
                    planowa_godzina_odjazdu = table.Column<TimeOnly>(type: "time", nullable: false),
                    offset_dni = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    nr_kursu = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odjazdy_Planowe", x => x.id_odjazdu);
                    table.ForeignKey(
                        name: "FK_Odjazdy_Planowe_Przystanki_Wariantu_id_przystanku_wariantu",
                        column: x => x.id_przystanku_wariantu,
                        principalTable: "Przystanki_Wariantu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Odjazdy_Planowe_Rozklady_Jazdy_id_rozkladu",
                        column: x => x.id_rozkladu,
                        principalTable: "Rozklady_Jazdy",
                        principalColumn: "id_rozkladu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grafiki_Pracy",
                columns: table => new
                {
                    id_grafiku = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_kierowcy = table.Column<int>(type: "integer", nullable: false),
                    id_pojazdu = table.Column<int>(type: "integer", nullable: false),
                    data = table.Column<DateOnly>(type: "date", nullable: false),
                    godzina_od = table.Column<TimeOnly>(type: "time", nullable: false),
                    godzina_do = table.Column<TimeOnly>(type: "time", nullable: false),
                    offset_dni = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grafiki_Pracy", x => x.id_grafiku);
                    table.ForeignKey(
                        name: "FK_Grafiki_Pracy_Kierowcy_id_kierowcy",
                        column: x => x.id_kierowcy,
                        principalTable: "Kierowcy",
                        principalColumn: "id_kierowcy",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grafiki_Pracy_Pojazdy_id_pojazdu",
                        column: x => x.id_pojazdu,
                        principalTable: "Pojazdy",
                        principalColumn: "id_pojazdu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Przeglady_Usterki",
                columns: table => new
                {
                    id_przegladu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_pojazdu = table.Column<int>(type: "integer", nullable: false),
                    id_typu_przegladu = table.Column<int>(type: "integer", nullable: false),
                    data_przegladu = table.Column<DateOnly>(type: "date", nullable: false),
                    wynik = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    uwagi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    warsztat = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Przeglady_Usterki", x => x.id_przegladu);
                    table.ForeignKey(
                        name: "FK_Przeglady_Usterki_Pojazdy_id_pojazdu",
                        column: x => x.id_pojazdu,
                        principalTable: "Pojazdy",
                        principalColumn: "id_pojazdu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Przeglady_Usterki_Typy_Przegladu_id_typu_przegladu",
                        column: x => x.id_typu_przegladu,
                        principalTable: "Typy_Przegladu",
                        principalColumn: "id_typu_przegladu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Realizacja_Kursu",
                columns: table => new
                {
                    id_realizacji = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_wariantu = table.Column<int>(type: "integer", nullable: false),
                    id_kierowcy = table.Column<int>(type: "integer", nullable: true),
                    id_pojazdu = table.Column<int>(type: "integer", nullable: true),
                    data_kursu = table.Column<DateOnly>(type: "date", nullable: false),
                    nr_kursu = table.Column<int>(type: "integer", nullable: false),
                    status_kursu = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Realizacja_Kursu", x => x.id_realizacji);
                    table.ForeignKey(
                        name: "FK_Realizacja_Kursu_Kierowcy_id_kierowcy",
                        column: x => x.id_kierowcy,
                        principalTable: "Kierowcy",
                        principalColumn: "id_kierowcy");
                    table.ForeignKey(
                        name: "FK_Realizacja_Kursu_Pojazdy_id_pojazdu",
                        column: x => x.id_pojazdu,
                        principalTable: "Pojazdy",
                        principalColumn: "id_pojazdu");
                    table.ForeignKey(
                        name: "FK_Realizacja_Kursu_Warianty_Trasy_id_wariantu",
                        column: x => x.id_wariantu,
                        principalTable: "Warianty_Trasy",
                        principalColumn: "id_wariantu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dziennik_Przejazdu",
                columns: table => new
                {
                    id_wpisu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_realizacji = table.Column<int>(type: "integer", nullable: false),
                    id_odjazdu = table.Column<int>(type: "integer", nullable: false),
                    rzeczywisty_odjazd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    rzeczywisty_przyjazd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    opoznienie_min = table.Column<int>(type: "integer", nullable: true),
                    offset_dni = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dziennik_Przejazdu", x => x.id_wpisu);
                    table.ForeignKey(
                        name: "FK_Dziennik_Przejazdu_Odjazdy_Planowe_id_odjazdu",
                        column: x => x.id_odjazdu,
                        principalTable: "Odjazdy_Planowe",
                        principalColumn: "id_odjazdu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dziennik_Przejazdu_Realizacja_Kursu_id_realizacji",
                        column: x => x.id_realizacji,
                        principalTable: "Realizacja_Kursu",
                        principalColumn: "id_realizacji",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kontrole_W_Pojazdach",
                columns: table => new
                {
                    id_kontroli = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_kontrolera = table.Column<int>(type: "integer", nullable: false),
                    id_realizacji = table.Column<int>(type: "integer", nullable: false),
                    data_godzina = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    wynik = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kontrole_W_Pojazdach", x => x.id_kontroli);
                    table.ForeignKey(
                        name: "FK_Kontrole_W_Pojazdach_Kontrolerzy_id_kontrolera",
                        column: x => x.id_kontrolera,
                        principalTable: "Kontrolerzy",
                        principalColumn: "id_kontrolera",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kontrole_W_Pojazdach_Realizacja_Kursu_id_realizacji",
                        column: x => x.id_realizacji,
                        principalTable: "Realizacja_Kursu",
                        principalColumn: "id_realizacji",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mandaty",
                columns: table => new
                {
                    id_mandatu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_kontroli = table.Column<int>(type: "integer", nullable: false),
                    id_pasazera = table.Column<int>(type: "integer", nullable: true),
                    nr_dokumentu_pasazera = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    kwota = table.Column<decimal>(type: "numeric(8,2)", nullable: false),
                    powod = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    data_platnosci = table.Column<DateOnly>(type: "date", nullable: true),
                    oplacony = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mandaty", x => x.id_mandatu);
                    table.ForeignKey(
                        name: "FK_Mandaty_Kontrole_W_Pojazdach_id_kontroli",
                        column: x => x.id_kontroli,
                        principalTable: "Kontrole_W_Pojazdach",
                        principalColumn: "id_kontroli",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mandaty_Pasazerowie_id_pasazera",
                        column: x => x.id_pasazera,
                        principalTable: "Pasazerowie",
                        principalColumn: "id_pasazera");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Badania_Lekarskie_id_kierowcy",
                table: "Badania_Lekarskie",
                column: "id_kierowcy");

            migrationBuilder.CreateIndex(
                name: "IX_Bilety_Okresowe_id_kategorii",
                table: "Bilety_Okresowe",
                column: "id_kategorii");

            migrationBuilder.CreateIndex(
                name: "IX_Bilety_Okresowe_id_pasazera",
                table: "Bilety_Okresowe",
                column: "id_pasazera");

            migrationBuilder.CreateIndex(
                name: "IX_Dzielnice_nazwa",
                table: "Dzielnice",
                column: "nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dziennik_Przejazdu_id_odjazdu",
                table: "Dziennik_Przejazdu",
                column: "id_odjazdu");

            migrationBuilder.CreateIndex(
                name: "IX_Dziennik_Przejazdu_id_realizacji",
                table: "Dziennik_Przejazdu",
                column: "id_realizacji");

            migrationBuilder.CreateIndex(
                name: "IX_Grafiki_Pracy_id_kierowcy",
                table: "Grafiki_Pracy",
                column: "id_kierowcy");

            migrationBuilder.CreateIndex(
                name: "IX_Grafiki_Pracy_id_pojazdu",
                table: "Grafiki_Pracy",
                column: "id_pojazdu");

            migrationBuilder.CreateIndex(
                name: "IX_Kategorie_Biletow_nazwa",
                table: "Kategorie_Biletow",
                column: "nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kierowcy_nr_pracownika",
                table: "Kierowcy",
                column: "nr_pracownika",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kontrole_W_Pojazdach_id_kontrolera",
                table: "Kontrole_W_Pojazdach",
                column: "id_kontrolera");

            migrationBuilder.CreateIndex(
                name: "IX_Kontrole_W_Pojazdach_id_realizacji",
                table: "Kontrole_W_Pojazdach",
                column: "id_realizacji");

            migrationBuilder.CreateIndex(
                name: "IX_Kontrolerzy_nr_sluzbowy",
                table: "Kontrolerzy",
                column: "nr_sluzbowy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Linie_numer_linii",
                table: "Linie",
                column: "numer_linii",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mandaty_id_kontroli",
                table: "Mandaty",
                column: "id_kontroli");

            migrationBuilder.CreateIndex(
                name: "IX_Mandaty_id_pasazera",
                table: "Mandaty",
                column: "id_pasazera");

            migrationBuilder.CreateIndex(
                name: "IX_Modele_Pojazdow_id_producenta",
                table: "Modele_Pojazdow",
                column: "id_producenta");

            migrationBuilder.CreateIndex(
                name: "IX_Odjazdy_Planowe_id_przystanku_wariantu",
                table: "Odjazdy_Planowe",
                column: "id_przystanku_wariantu");

            migrationBuilder.CreateIndex(
                name: "IX_Odjazdy_Planowe_id_rozkladu_id_przystanku_wariantu",
                table: "Odjazdy_Planowe",
                columns: new[] { "id_rozkladu", "id_przystanku_wariantu" });

            migrationBuilder.CreateIndex(
                name: "IX_Pasazerowie_email",
                table: "Pasazerowie",
                column: "email",
                unique: true,
                filter: "email IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Pasazerowie_pesel",
                table: "Pasazerowie",
                column: "pesel",
                unique: true,
                filter: "pesel IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Pojazdy_id_modelu",
                table: "Pojazdy",
                column: "id_modelu");

            migrationBuilder.CreateIndex(
                name: "IX_Pojazdy_numer_boczny",
                table: "Pojazdy",
                column: "numer_boczny",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pojazdy_vin",
                table: "Pojazdy",
                column: "vin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producenci_Pojazdow_nazwa",
                table: "Producenci_Pojazdow",
                column: "nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Przeglady_Usterki_id_pojazdu",
                table: "Przeglady_Usterki",
                column: "id_pojazdu");

            migrationBuilder.CreateIndex(
                name: "IX_Przeglady_Usterki_id_typu_przegladu",
                table: "Przeglady_Usterki",
                column: "id_typu_przegladu");

            migrationBuilder.CreateIndex(
                name: "IX_Przystanki_id_dzielnicy",
                table: "Przystanki",
                column: "id_dzielnicy");

            migrationBuilder.CreateIndex(
                name: "IX_Przystanki_Wariantu_id_przystanku",
                table: "Przystanki_Wariantu",
                column: "id_przystanku");

            migrationBuilder.CreateIndex(
                name: "IX_Przystanki_Wariantu_id_wariantu_id_przystanku",
                table: "Przystanki_Wariantu",
                columns: new[] { "id_wariantu", "id_przystanku" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Przystanki_Wariantu_id_wariantu_kolejnosc",
                table: "Przystanki_Wariantu",
                columns: new[] { "id_wariantu", "kolejnosc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Realizacja_Kursu_id_kierowcy",
                table: "Realizacja_Kursu",
                column: "id_kierowcy");

            migrationBuilder.CreateIndex(
                name: "IX_Realizacja_Kursu_id_pojazdu",
                table: "Realizacja_Kursu",
                column: "id_pojazdu");

            migrationBuilder.CreateIndex(
                name: "IX_Realizacja_Kursu_id_wariantu_data_kursu_nr_kursu",
                table: "Realizacja_Kursu",
                columns: new[] { "id_wariantu", "data_kursu", "nr_kursu" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rozklady_Jazdy_id_typu_dnia",
                table: "Rozklady_Jazdy",
                column: "id_typu_dnia");

            migrationBuilder.CreateIndex(
                name: "IX_Rozklady_Jazdy_id_wariantu",
                table: "Rozklady_Jazdy",
                column: "id_wariantu");

            migrationBuilder.CreateIndex(
                name: "IX_Typy_Dni_kod",
                table: "Typy_Dni",
                column: "kod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Typy_Przegladu_kod",
                table: "Typy_Przegladu",
                column: "kod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Uprawnienia_Kategorii_kategoria",
                table: "Uprawnienia_Kategorii",
                column: "kategoria",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Uprawnienia_Kierowcow_id_kierowcy",
                table: "Uprawnienia_Kierowcow",
                column: "id_kierowcy");

            migrationBuilder.CreateIndex(
                name: "IX_Uprawnienia_Kierowcow_id_uprawnienia",
                table: "Uprawnienia_Kierowcow",
                column: "id_uprawnienia");

            migrationBuilder.CreateIndex(
                name: "IX_Warianty_Trasy_id_linii_nazwa_wariantu",
                table: "Warianty_Trasy",
                columns: new[] { "id_linii", "nazwa_wariantu" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Badania_Lekarskie");

            migrationBuilder.DropTable(
                name: "Bilety_Okresowe");

            migrationBuilder.DropTable(
                name: "Dziennik_Przejazdu");

            migrationBuilder.DropTable(
                name: "Grafiki_Pracy");

            migrationBuilder.DropTable(
                name: "Mandaty");

            migrationBuilder.DropTable(
                name: "Przeglady_Usterki");

            migrationBuilder.DropTable(
                name: "Uprawnienia_Kierowcow");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Kategorie_Biletow");

            migrationBuilder.DropTable(
                name: "Odjazdy_Planowe");

            migrationBuilder.DropTable(
                name: "Kontrole_W_Pojazdach");

            migrationBuilder.DropTable(
                name: "Pasazerowie");

            migrationBuilder.DropTable(
                name: "Typy_Przegladu");

            migrationBuilder.DropTable(
                name: "Uprawnienia_Kategorii");

            migrationBuilder.DropTable(
                name: "Przystanki_Wariantu");

            migrationBuilder.DropTable(
                name: "Rozklady_Jazdy");

            migrationBuilder.DropTable(
                name: "Kontrolerzy");

            migrationBuilder.DropTable(
                name: "Realizacja_Kursu");

            migrationBuilder.DropTable(
                name: "Przystanki");

            migrationBuilder.DropTable(
                name: "Typy_Dni");

            migrationBuilder.DropTable(
                name: "Kierowcy");

            migrationBuilder.DropTable(
                name: "Pojazdy");

            migrationBuilder.DropTable(
                name: "Warianty_Trasy");

            migrationBuilder.DropTable(
                name: "Dzielnice");

            migrationBuilder.DropTable(
                name: "Modele_Pojazdow");

            migrationBuilder.DropTable(
                name: "Linie");

            migrationBuilder.DropTable(
                name: "Producenci_Pojazdow");
        }
    }
}
