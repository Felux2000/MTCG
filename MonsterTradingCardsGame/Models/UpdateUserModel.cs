namespace MonsterTradingCardsGame.Models
{
    internal class UpdateUserModel
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        public UpdateUserModel(string Name, string Bio, string Image)
        {
            this.Name = Name;
            this.Bio = Bio;
            this.Image = Image;
        }
    }
}
