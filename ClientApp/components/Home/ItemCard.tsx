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

    useEffect(() => {
      // Assuming you have a utility function to determine the item status
      const calculateItemStatus = (item: Item): string => {
        const currentDate = new Date();
  
        const endDate = item.auctionHistory?.winner != null 
            ? item.auctionHistory.endDate 
            : item.endDate;
      
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
      <div className="bg-white shadow-md rounded-lg p-4 transition-transform transform hover:scale-105 relative"
        onClick={() => router.push(`/items/${item.itemId}`)}
      >
        <div className='-right-3 -top-3 absolute p-3 bg-meta-3 text-black-2 rounded-3xl'>
          <p>{itemStatus}</p>
        </div>
        <h5 className="text-xl font-bold mb-2">{item.title}</h5>
        <p className="text-gray-600 mb-4">{item.description}</p>
        <img src={item.image} alt={item.title} className="w-full mb-4" />
        <p className="text-gray-600 mb-4">Current Price: ${HighestBid}</p>
        {item.sellerId && (
          <div className="flex items-center mb-4">
            <img src={item.seller?.avatar} alt="" className='w-5 h-5'/>
            <p className="text-gray-600 mb-2">{item.seller?.name}</p>
          </div>
        )}
        {winner ? <div className='flex flex-row justify-center items-center w-full bg-meta-3 text-black-2 px-2 py-1'>
          <p className='mx-3'>winner: </p>
            <img src={winner?.avatar} alt="" className='w-5 h-5'/>
            <p className='text-lg'>{winner?.name}</p>
          </div> : (
            <p className='text-black-2'>No Winner Yet</p>
          )}
      </div>
    );
  };
  
export default ItemCard