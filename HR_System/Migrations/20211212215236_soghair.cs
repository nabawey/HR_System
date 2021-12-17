using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_System.Migrations
{
    public partial class soghair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "vacation_name",
                table: "Vacation",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "workedHours",
                table: "Att_dep",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "DatePart(HOUR ,[departure] ) + DatePart(MINUTE ,[departure])/60.0 -  DatePart(HOUR ,[attendance] ) + DatePart(MINUTE ,[attendance] )/60.0",
                oldClrType: typeof(float),
                oldType: "real",
                oldComputedColumnSql: "DatePart(HOUR ,[departure] ) + DatePart(MINUTE ,[departure])/60.0 -  DatePart(HOUR ,[attendance] ) + DatePart(MINUTE ,[attendance] )/60.0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "vacation_name",
                table: "Vacation",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<float>(
                name: "workedHours",
                table: "Att_dep",
                type: "real",
                nullable: false,
                computedColumnSql: "DatePart(HOUR ,[departure] ) + DatePart(MINUTE ,[departure])/60.0 -  DatePart(HOUR ,[attendance] ) + DatePart(MINUTE ,[attendance] )/60.0",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "DatePart(HOUR ,[departure] ) + DatePart(MINUTE ,[departure])/60.0 -  DatePart(HOUR ,[attendance] ) + DatePart(MINUTE ,[attendance] )/60.0");
        }
    }
}
