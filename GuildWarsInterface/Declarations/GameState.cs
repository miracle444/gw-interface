namespace GuildWarsInterface.Declarations
{
        public enum GameState
        {
                Handshake, // must be first element (0)
                LoginScreen,
                CharacterScreen,
                LoadingScreen,
                Playing,
                ChangingMap,
                CharacterCreation
        }
}