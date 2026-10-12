namespace UserManagement.Infrastructure.Persistence;

internal static class SeedData
{
    public const int UsersReadPermissionId = 1;
    public const int UsersUpdatePermissionId = 2;

    public const int AdministratorRoleId = 1;
    public const int ConsultaRoleId = 2;

    public const int AdminUserId = 1;
    public const int ConsultaUserId = 2;
    public const int InactiveUserId = 3;

    public const string AdminPasswordHash = "$2a$11$2mRhyT3nfQ8H9S6/H1nqPOZ2SLQ71kySrjJY4XDtZYMm2i7Ct1s8a";
    public const string ConsultaPasswordHash = "$2a$11$kWmpopRaDkrQtxb6cgu5AuVKoskwfefg/4d1CQeRsqQ8748I9oikS";
    public const string InactivePasswordHash = "$2a$11$JacUlUEvjJr.IlAiKN8ZbOfA4pbCd.VJwy96nF5cCJVcbIoFKmF0e";
}
