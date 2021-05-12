using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;


namespace InGas.UnitTest
{
    /// <summary>
    ///     An interface for building.
    /// </summary>
    public interface IBuilder { }

    /// <summary>
    ///     Default methods for the <see cref="IBuilder" /> abstraction.
    /// </summary>
    public static class IBuilderExtensions
    {
        /// <summary>
        ///     Adds the specified field to the builder.
        /// </summary>
        /// <typeparam name="TBuilder">The type of the builder.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="builder">This builder.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>The builder.</returns>
        public static TBuilder With<TBuilder, TField>(this TBuilder builder, ref TField field, TField value)
            where TBuilder : IBuilder
        {
            if (typeof(TField).IsValueType && !typeof(TField).IsNullableType() && field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            field = value;

            return builder;
        }

        /// <summary>
        ///     Adds the specified list of fields to the builder.
        /// </summary>
        /// <typeparam name="TBuilder">The type of the builder.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="builder">This builder.</param>
        /// <param name="field">The field.</param>
        /// <param name="values">The values.</param>
        /// <returns>The builder.</returns>
        public static TBuilder With<TBuilder, TField>(this TBuilder builder,
                                                      ref List<TField> field,
                                                      IEnumerable<TField> values)
            where TBuilder : IBuilder
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (values == null)
            {
                field = null;
            }
            else
            {
                field.AddRange(values);
            }

            return builder;
        }

        /// <summary>
        /// Adds the specified <see cref="value"/> to the builder with a specified number of repetitions
        /// </summary>
        /// <typeparam name="TBuilder">The type of the builder</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="builder">This builder.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value to repeat insert</param>
        /// <param name="repeat">The number of times to insert. Defaults to 1</param>
        /// <returns>The builder.</returns>
        public static TBuilder With<TBuilder, TField>(this TBuilder builder,
                                                      ref IEnumerable<TField> field,
                                                      TField value,
                                                      int repeat = 1)
            where TBuilder : IBuilder
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            field = value != null
                        ? Enumerable.Repeat(value, repeat)
                        : null;

            return builder;
        }

        /// <summary>
        ///     Adds the specified field to the builder.
        /// </summary>
        /// <typeparam name="TBuilder">The type of the builder.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="builder">This builder.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>The builder.</returns>
        public static TBuilder With<TBuilder, TField>(this TBuilder builder, ref List<TField> field, TField value)
            where TBuilder : IBuilder
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            field.Add(value);

            return builder;
        }

        /// <summary>
        ///     Adds the specified key value pair to the provided dictionary.
        /// </summary>
        /// <typeparam name="TBuilder">The type of the builder.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="builder">This builder.</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyValuePair">The key value pair.</param>
        /// <returns>The builder.</returns>
        public static TBuilder With<TBuilder, TKey, TField>(this TBuilder builder,
                                                            ref Dictionary<TKey, TField> dictionary,
                                                            KeyValuePair<TKey, TField> keyValuePair)
            where TBuilder : IBuilder
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            dictionary.Add(keyValuePair.Key, keyValuePair.Value);

            return builder;
        }

        /// <summary>
        ///     Adds the specified key and value to the provided dictionary.
        /// </summary>
        /// <typeparam name="TBuilder">The type of the builder.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="builder">This builder.</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The builder.</returns>
        public static TBuilder With<TBuilder, TKey, TField>(this TBuilder builder,
                                                            ref Dictionary<TKey, TField> dictionary,
                                                            TKey key,
                                                            TField value)
            where TBuilder : IBuilder
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            dictionary.Add(key, value);

            return builder;
        }

        /// <summary>
        ///     Adds the specified dictionary to the provided dictionary.
        /// </summary>
        /// <typeparam name="TBuilder">The type of the builder.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="builder">This builder.</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyValuePair">The key value pair.</param>
        /// <returns> The builder.</returns>
        public static TBuilder With<TBuilder, TKey, TField>(this TBuilder builder,
                                                            ref Dictionary<TKey, TField> dictionary,
                                                            IDictionary<TKey, TField> keyValuePair)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            dictionary = (Dictionary<TKey, TField>)keyValuePair;

            return builder;
        }
    }
}