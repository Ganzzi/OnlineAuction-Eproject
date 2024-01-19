"use client"

import BidCard from '@/components/Home/market/BidCard';
import { useGlobalState } from '@/context/globalState';
import { Item } from '@/types/models/item'
import { User } from '@/types/models/user';
import { parseDate } from '@/utils';
import { useRouter } from 'next/navigation';
import React, { useEffect, useState } from 'react'


const Index = ({ itemData }: { itemData: Item }) => {
  const router = useRouter();
  const { user } = useGlobalState(); // replace with your authentication context

  const [item, setItem] = useState<Item>(itemData);
  const [bidAmount, setBidAmount] = useState('');
  const [ratingAmount, setRatingAmount] = useState<number>();
  const [itemStatus, setItemStatus] = useState<string>(''); // 'started', 'not started', 'ended'
  const [winner, setWinner] = useState<User | null>(null);
  
  useEffect(() => {
    // Assuming you have a utility function to determine the item status
    const calculateItemStatus = (item: Item): string => {
      
      const currentDate = new Date();
      if (currentDate < parseDate(item.startDate)) {
        return 'not started';
      } else if (currentDate >= parseDate(item.startDate) && currentDate <= parseDate(item.endDate)) {
        return 'started';
      } else {
        return 'ended';
      }
    };

    setItemStatus(calculateItemStatus(itemData));

    setWinner(itemData?.auctionHistory?.winner ?? null);
   
  }, [itemData]);

  const isItemSeller = user && item?.seller?.userId === user.userId;
  
  return (
    <div className="flex flex-col md:flex-row p-4 gap-8">
      <div className="md:w-1/2">
        {item && (
          <>
            <img src={item.image} alt={item.title} className="mb-4" />
            <h1 className="text-3xl font-bold mb-4">{item.title}</h1>
            <p className="text-gray-600 mb-4">{item.description}</p>
            <p className="text-lg font-semibold mb-4">Price: ${item.startingPrice}</p>
            <p className="text-lg font-semibold mb-4">Increasing Amount: ${item.increasingAmount}</p>
            <p className="text-lg font-semibold mb-4">Status: {itemStatus}</p>
            {winner && <p className="text-lg font-semibold mb-4">Winner: {winner.name}</p>}

            {/* Display other item details as needed */}
          </>
        )}
      </div>

      <div className="md:w-1/2">
        {/* Bid Form */}
        {user && isItemSeller &&  itemStatus === 'not started'  && (
          <div className="mb-8">
            <button
              onClick={() => router.push(`/items/form?itemId=${item.itemId}`)}
              className="mt-2 px-4 py-2 bg-meta-5 hover:bg-meta-3 text-white rounded hover:bg-blue-600"
            >
              Edit
            </button>
          </div>
        )}

        {user && !isItemSeller && itemStatus === 'started' && (
          <div className="mb-8">
            <label htmlFor="bidAmount" className="block text-sm font-medium text-gray-600">
              Your Bid Amount:
            </label>
            <input
              type="number"
              id="bidAmount"
              value={bidAmount}
              onChange={(e) => setBidAmount(e.target.value)}
              className="mt-1 p-2 border rounded w-full"
            />
            <button
              onClick={() => router.push("/items/bid")}
              className="mt-2 px-4 py-2 bg-meta-5 hover:bg-meta-3 text-white rounded hover:bg-blue-600"
            >
              Place Bid
            </button>
          </div>
        )}

        {/* List of Bids */}
        <div>
          <h2 className="text-xl font-semibold mb-4">List of Bids:</h2>
          <ul className="overflow-y-auto max-h-187.5">
            {item?.bids?.map((bid) => (
              <li key={bid.bidId} className="mb-2">
                <BidCard bid={bid} />
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
};

export default Index