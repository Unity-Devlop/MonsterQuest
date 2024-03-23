using System;
using MemoryPack;
using UnityEngine.Assertions;

namespace Game
{
    [Serializable]//, MemoryPackable]
    public readonly partial struct FriendShip : IEquatable<FriendShip>
    {
        public readonly string uid1;
        public readonly string uid2;
        public readonly string playerName1;
        public readonly string playerName2;

        public FriendShip(string uid1, string uid2, string playerName1, string playerName2) //: this()
        {
            Assert.IsFalse(uid1 == uid2);
            this.uid1 = uid1;
            this.uid2 = uid2;
            GetSmaller(uid1, uid2, out string smaller1, out string smaller2);
            this.uid1 = smaller1;
            this.uid2 = smaller2;
            if (uid1 == smaller1)
            {
                this.playerName1 = playerName1;
                this.playerName2 = playerName2;
            }
            else
            {
                this.playerName1 = playerName2;
                this.playerName2 = playerName1;
            }
        }

        public override int GetHashCode()
        {
            return uid1.GetHashCode() ^ uid2.GetHashCode();
        }

 
        public bool Equals(FriendShip other)
        {
            return uid1 == other.uid1 && uid2 == other.uid2;
        }

        public override bool Equals(object obj)
        {
            return obj is FriendShip other && Equals(other);
        }

        private static void GetSmaller(string uid1, string uid2, out string smaller1, out string smaller2)
        {
            smaller1 = uid1;
            smaller2 = uid2;
            if (String.Compare(uid1, uid2, StringComparison.Ordinal) > 0)
            {
                smaller1 = uid2;
                smaller2 = uid1;
            }
        }
    }

    [Serializable]//, MemoryPackable]
    public readonly struct FriendPair
    {
        public readonly string uid;
        public readonly string playerName;

        public FriendPair(string uid, string playerName)
        {
            this.uid = uid;
            this.playerName = playerName;
        }
    }
}