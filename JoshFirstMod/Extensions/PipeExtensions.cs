using System;

namespace JoshFirstMod.Extensions
{
    public static class PipeExtensions
    {
        /// <summary>
        /// Pass input to func and return the result.
        /// </summary>
        /// <typeparam name="TParam">Parameter type.</typeparam>
        /// <typeparam name="TOutput">The type asyncFunc returns</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        /// <returns>An object of type U</returns>
        public static TOutput Pipe<TParam, TOutput>(this TParam input, Func<TParam, TOutput> func)
            => func(input);
    }
}
