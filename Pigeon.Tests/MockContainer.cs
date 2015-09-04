namespace Pigeon.Tests
{
    using Data.Contracts;
    using Models;
    using Moq;

    public class MockContainer
    {
        /* TODO: The Mocking test could be time demanding, because of the all aditional setup that we should make.
         * Another thing is that we should mock users, which will affect the source code in the Controllers and 
         * that could misslead the other memers of the team, that is why the mocking test are considered as optional. */

        public Mock<IPigeonRepository<Pigeon>> PigeonRepositoryMock { get; set; }

        public Mock<IPigeonRepository<Comment>> CommentRepositoryMock { get; set; }

        public Mock<IPigeonRepository<Notification>> NotificationRepositoryMock { get; set; }

        public Mock<IPigeonRepository<Photo>> PhotoRepositoryMock { get; set; }

        public Mock<IPigeonRepository<PigeonVote>> PigeonVoteRepositoryMock { get; set; }

        public Mock<IPigeonRepository<User>> UserRepositoryMock { get; set; }

        public Mock<IPigeonRepository<UserSession>> UserSessionRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            //TODO: Setup the fake data.
        }
    }
}
