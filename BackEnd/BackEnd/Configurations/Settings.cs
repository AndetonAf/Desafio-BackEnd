namespace BackEnd.Configurations
{
    public record Settings(Auth Auth, ConnectionStrings ConnectionStrings, Gcp Gcp);
    public record Auth(string SecretJwt);
    public record ConnectionStrings(string Db);
    public record Gcp(bool PathRelative, string PathJson, string ProjectId, string Bucket);
}
