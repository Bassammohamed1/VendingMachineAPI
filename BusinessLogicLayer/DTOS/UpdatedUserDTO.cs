namespace BusinessLogicLayer.DTOS
{
    public class UpdatedUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public double? Deposit { get; set; }
        public string Role { get; set; }
    }
}
