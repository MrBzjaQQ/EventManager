namespace EventManager.Dtos.User
{
    public class UserLoginSuccessDto
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string UserName { get; set; }
        
        public string AccessToken { get; set; }
    }
}