import useColorMode from '@/hooks/useColorMode';
import { Bid } from '@/types/models/bid';
import { Item } from '@/types/models/item';
import { User } from '@/types/models/user';
import { parseDate } from '@/utils';
import { useRouter } from 'next/navigation';
import React, { useEffect, useState } from 'react'


const ItemCard = ({ item }: { item: Item }) => {
  const router = useRouter()
  const HighestBid = item.bids && item.bids.length != 0 ? item.bids[0]?.bidAmount : item.increasingAmount;
  const [itemStatus, setItemStatus] = useState<string>(''); // 'started', 'not started', 'ended'
  const [winner, setWinner] = useState<User | null>(null);

  const endDate = item.auctionHistory?.winner != null
    ? item.auctionHistory.endDate
    : item.endDate;
  useEffect(() => {
    // Assuming you have a utility function to determine the item status
    const calculateItemStatus = (item: Item): string => {
      const currentDate = new Date();


      if (currentDate < parseDate(item.startDate)) {
        return 'not started';
      } else if (currentDate >= parseDate(item.startDate) && currentDate <= parseDate(endDate)) {
        return 'started';
      } else {
        return 'ended';
      }
    };

    setItemStatus(calculateItemStatus(item));


    setWinner(item?.auctionHistory?.winner ?? null);

  }, [item]);

  return (
    <div className="bg-white shadow-md rounded-lg p-4 transition-transform transform hover:scale-105 relative mx-3 my-2"
      onClick={() => router.push(`/items/${item.itemId}`)}
    >
      <div className='-right-2 top-20 absolute p-3 bg-meta-3 text-black-2 rounded-3xl'>
        {itemStatus == "started" && (
          <div>
            <p className='text-font-bold text-lg'>Auction End in</p>
            <Countdown date={endDate} />
          </div>
        )}

        {itemStatus == "not started" && (
          <div>
            <p className='text-font-bold text-lg'>Auction Start in</p>
            <Countdown date={item.startDate} />
          </div>
        )}

        {itemStatus == "ended" && (
          <div>
            <p className='text-font-bold text-lg'>Auction Ended</p>
          </div>
        )}
      </div>
      <h5 className="text-2xl text-black font-bold mb-2">{item.title}</h5>
      <p className="text-body mb-4">{item.description}</p>
      <img src={item.image} alt={item.title} className="h-1/2 w-auto mb-4" />
      <p className="text-body mb-4">Current Price: <span className='text-meta-1 text-lg'>${HighestBid}</span></p>
      <div className='flex flex-row justify-between'>
        <div>
      {item.sellerId && (
        <div className="flex items-center mb-4">
          <img src={item.seller?.avatar} alt="" className='w-5 h-5' />
          <p className="text-body mb-2">{item.seller?.name}</p>
        </div>
      )}
      </div>

      <div>
        <p className='text-2xl text-black '>{item.bids?.length} Bids Placed</p>
      </div>

    </div>
      {winner ? <div className='flex flex-row justify-center items-center w-full bg-meta-3 text-black-2 px-2 py-1'>
        <p className='mx-3'>winner: </p>
        <img src={winner?.avatar} alt="" className='w-5 h-5' />
        <p className='text-lg'>{winner?.name}</p>
      </div> : (
        <div className='flex flex-row justify-center items-center w-full bg-meta-4 text-bodydark px-2 py-1'>
          No Winner Yet
        </div>
      )}
    </div>
  );
};

export const Countdown = ({ date }: { date: string }) => {
  const targetDate = new Date(date);
  const [timeRemaining, setTimeRemaining] = useState(calculateTimeRemaining());

  useEffect(() => {
    const intervalId = setInterval(() => {
      setTimeRemaining(calculateTimeRemaining());
    }, 1000);

    return () => clearInterval(intervalId);
  }, []);

  function calculateTimeRemaining() {
    const now = new Date();
    const difference = targetDate.getTime() - now.getTime();

    if (difference <= 0) {
      // Target date has passed
      return { days: 0, hours: 0, minutes: 0, seconds: 0 };
    }

    const days = Math.floor(difference / (1000 * 60 * 60 * 24));
    const hours = Math.floor((difference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((difference % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((difference % (1000 * 60)) / 1000);

    return { days, hours, minutes, seconds };
  }

  return (
    <div className='flex flex-row justify-center items-center'>
      {timeRemaining.days > 0 && <div>{timeRemaining.days}d-</div>}
      {timeRemaining.hours > 0 && <div>{timeRemaining.hours}h-</div>}
      {timeRemaining.minutes > 0 && <div>{timeRemaining.minutes}m-</div>}
      {timeRemaining.seconds > 0 && <div>{timeRemaining.seconds}s</div>}
      {timeRemaining.days === 0 && timeRemaining.hours === 0 && timeRemaining.minutes === 0 && timeRemaining.seconds === 0 && <div>Countdown expired</div>}
    </div>
  );
}
export default ItemCard