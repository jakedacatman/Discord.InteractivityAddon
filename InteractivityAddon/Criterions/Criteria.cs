using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace InteractivityAddon.Criterions
{
    /// <summary>
    /// Represents a collection of <see cref="ICriterion{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Criteria<T>
    {
        private ImmutableList<ICriterion<T>> Criterions { get; set; }
        public int CriterionAmount => Criterions.Count;

        public Criteria(List<ICriterion<T>> criterions)
        {
            Criterions = criterions.ToImmutableList();
        }

        public Criteria(params ICriterion<T>[] criterions)
        {
            Criterions = criterions.ToImmutableList();
        }

        public Criteria()
        {
            Criterions = new List<ICriterion<T>>().ToImmutableList();
        }

        internal void AddCriterion(ICriterion<T> criterion)
            => Criterions = Criterions.Add(criterion);

        public async Task<bool> JudgeAsync(T value)
        {
            foreach (var criterion in Criterions) {
                if (await criterion.JudgeAsync(value) == false) {
                    return false;
                }
            }

            return true;
        }

        public static Criteria<SocketMessage> GetMessageCriteria(IMessageChannel channel = null, SocketUser user = null)
        {
            var criteria = new Criteria<SocketMessage>();

            if (channel != null) {
                criteria.AddCriterion(new EnsureMessageChannel(channel.Id));
            }

            if (user != null) {
                criteria.AddCriterion(new EnsureMessageUser(user.Id));
            }

            return criteria;
        }
    }
}
