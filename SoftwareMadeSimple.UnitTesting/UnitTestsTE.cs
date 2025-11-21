namespace SoftwareMadeSimple.UnitTesting
{
    internal sealed class UnitTests
    {
        private struct SomeError;

        [Test]
        public void SameArgumentTypesSuccess()
        {
            var input = "This is a success";
            var success = SimpleResults.Result<string, string>.Success(input);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(success.IsSuccess, Is.True);
                Assert.That(success.IsFailure, Is.False);
                Assert.That(success.Value, Is.EqualTo(input));
                Assert.That(() => success.Error, Throws.InvalidOperationException);
            }
        }

        [Test]
        public void SameArgumentTypesError()
        {
            var msg = "This is a failure";
            var failure = SimpleResults.Result<string, string>.Failure(msg);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(failure.IsSuccess, Is.False);
                Assert.That(failure.IsFailure, Is.True);
                Assert.That(() => failure.Value, Throws.InvalidOperationException);
                Assert.That(failure.Error, Is.EqualTo(msg));
            }
        }

        [Test]
        public void Success()
        {
            var input = 42;
            var result = SimpleResults.Result<int, SomeError>.Success(input);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.IsFailure, Is.False);
                Assert.That(result.Value, Is.EqualTo(input));
                Assert.That(() => result.Error, Throws.InvalidOperationException);
            }
        }

        [Test]
        public void Failure()
        {
            var result = SimpleResults.Result<int, SomeError>.Failure(new SomeError());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.IsFailure, Is.True);
                Assert.That(() => result.Value, Throws.InvalidOperationException);
                Assert.That(result.Error, Is.EqualTo(new SomeError()));
            }
        }

        [Test]
        public void ImplicitError()
        {
            var failure = (SimpleResults.Result<int, SomeError>)new SomeError();

            Assert.That(failure.IsFailure, Is.True);
        }

        [Test]
        public void ImplicitSuccess()
        {
            var success = (SimpleResults.Result<int, SomeError>)42;

            Assert.That(success.IsSuccess, Is.True);
        }
    }
}