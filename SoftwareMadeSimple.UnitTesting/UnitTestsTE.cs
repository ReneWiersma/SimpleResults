namespace SoftwareMadeSimple.UnitTesting
{
    internal sealed class UnitTests
    {
        private struct SomeError;

        [Test]
        public void SameArgumentTypesSuccess()
        {
            var input = "This is a success";
            var result = SimpleResults.Result<string, string>.Success(input);

            AssertSuccess(input, result);
        }

        [Test]
        public void SameArgumentTypesError()
        {
            var input = "This is a failure";
            var result = SimpleResults.Result<string, string>.Failure(input);

            AssertFailure(input, result);
        }

        [Test]
        public void SuccessStaticCreator()
        {
            var input = 42;
            var result = SimpleResults.Result<int, SomeError>.Success(input);

            AssertSuccess(input, result);
        }

        [Test]
        public void FailureStaticCreator()
        {
            var input = new SomeError();
            var result = SimpleResults.Result<int, SomeError>.Failure(input);

            AssertFailure(input, result);
        }

        [Test]
        public void ImplicitError()
        {
            var input = new SomeError();
            var result = (SimpleResults.Result<int, SomeError>)input;

            Assert.That(result.IsFailure, Is.True);
        }

        [Test]
        public void ImplicitSuccess()
        {
            var input = 42;
            var result = (SimpleResults.Result<int, SomeError>)input;

            AssertSuccess(input, result);
        }

        [Test]
        public void SuccessConstructor()
        {
            var input = 42;
            var result = new SimpleResults.Success<int, SomeError>(input);

            AssertSuccess(input, result);
        }

        [Test]
        public void FailureConstructor()
        {
            var input = new SomeError();
            var result = new SimpleResults.Failure<int, SomeError>(input);

            AssertFailure(input, result);
        }

        private static void AssertSuccess<T, E>(T value, SimpleResults.Result<T, E> result)
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.IsFailure, Is.False);
                Assert.That(result.Value, Is.EqualTo(value));
                Assert.That(() => result.Error, Throws.InvalidOperationException.With.Message.EqualTo("Cannot access Error when result is a success."));
            }
        }

        private static void AssertFailure<T, E>(E error, SimpleResults.Result<T, E> result)
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.IsFailure, Is.True);
                Assert.That(() => result.Value, Throws.InvalidOperationException.With.Message.EqualTo("Cannot access Value when result is a failure."));
                Assert.That(result.Error, Is.EqualTo(error));
            }
        }
    }
}