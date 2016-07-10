using System;

namespace Functional
{
    public class Either<TLeft, TRight>
    {
        private bool? leftOrRight;
        private TLeft left;
        public TLeft Left
        {
            get
            {
                if (!leftOrRight.HasValue || !leftOrRight.Value) throw new ArgumentNullException();
                return left;
            }
        }

        private TRight right;
        public TRight Right
        {
            get
            {
                if (!leftOrRight.HasValue || leftOrRight.Value) throw new ArgumentNullException();
                return right;
            }
        }

        public bool IsLeft
        {
            get
            {
                if (!leftOrRight.HasValue) throw new ArgumentNullException();
                return leftOrRight.Value;
            }
        }

        public bool IsRight
        {
            get
            {
                if (!leftOrRight.HasValue) throw new ArgumentNullException();
                return !leftOrRight.Value;
            }
        }

        public T Match<T>(Func<TLeft, T> leftPredicate, Func<TRight, T> rightPredicate)
        {
            return IsLeft ? leftPredicate(left) : rightPredicate(right);
        }

        public void Match<T>(Action<TLeft> leftPredicate, Action<TRight> rightPredicate)
        {
            if (IsLeft)
                leftPredicate(left);
            else
                rightPredicate(right);
        }

        private Either(TLeft value)
        {
            this.leftOrRight = true;
            this.left = value;
        }
        private Either(TRight value)
        {
            this.leftOrRight = false;
            this.right = value;
        }

        public static implicit operator Either<TLeft, TRight>(TLeft left)
        {
            return new Either<TLeft, TRight>(left);
        }

        public static implicit operator Either<TLeft, TRight>(TRight right)
        {
            return new Either<TLeft, TRight>(right);
        }
    }

    public static class EitherExt
    {
        public static Either<TLeftNew, TRight> Select<TLeft, TLeftNew, TRight>(this Either<TLeft, TRight> self, Func<TLeft, TLeftNew> mapper)
        {
            if (self.IsRight) return self.Right;
            return mapper(self.Left);
        }

        public static Either<TLeftNew, TRightNew> Bind<TLeft, TRight, TLeftNew, TRightNew>(this Either<TLeft, TRight> self, Func<TLeft, Either<TLeftNew, TRightNew>> binder)
            where TRight : TRightNew
        {
            if (self.IsRight) return self.Right;
            return binder(self.Left);
        }
    }
}
