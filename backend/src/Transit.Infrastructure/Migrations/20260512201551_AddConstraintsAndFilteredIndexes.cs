using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraintsAndFilteredIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Unikalny indeks: jeden rekord uprawnienia danej kategorii na kierowcę
            // (CURRENT_DATE nie może być w predykacie — volatile function)
            migrationBuilder.Sql("""
                CREATE UNIQUE INDEX IF NOT EXISTS uq_uprawnienie_kierowcy
                ON "Uprawnienia_Kierowcow" (id_kierowcy, id_uprawnienia);
                """);

            // CHECK: realizacja może być 'zrealizowana' tylko gdy przypisano kierowcę i pojazd
            migrationBuilder.Sql("""
                ALTER TABLE "Realizacja_Kursu"
                ADD CONSTRAINT chk_zrealizowany_wymaga_kierowcy_i_pojazdu
                CHECK (
                    status_kursu <> 'zrealizowany'
                    OR (id_kierowcy IS NOT NULL AND id_pojazdu IS NOT NULL)
                );
                """);

            // CHECK: mandat musi mieć pasażera lub numer dokumentu
            migrationBuilder.Sql("""
                ALTER TABLE "Mandaty"
                ADD CONSTRAINT chk_mandat_pasazer_lub_dokument
                CHECK (id_pasazera IS NOT NULL OR nr_dokumentu_pasazera IS NOT NULL);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""DROP INDEX IF EXISTS uq_uprawnienie_kierowcy;""");
            migrationBuilder.Sql("""
                ALTER TABLE "Realizacja_Kursu"
                DROP CONSTRAINT IF EXISTS chk_zrealizowany_wymaga_kierowcy_i_pojazdu;
                """);
            migrationBuilder.Sql("""
                ALTER TABLE "Mandaty"
                DROP CONSTRAINT IF EXISTS chk_mandat_pasazer_lub_dokument;
                """);
        }
    }
}
