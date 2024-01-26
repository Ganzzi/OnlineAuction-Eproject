"use client"

import axiosService from '@/services/axiosService';
import BidCard from '@/components/Home/market/BidCard';
import { useGlobalState } from '@/context/globalState';
import { Item } from '@/types/models/item'
import { User } from '@/types/models/user';
import { parseDate } from '@/utils';
import { useRouter } from 'next/navigation';
import React, { use, useEffect, useState } from 'react'
import signalRService from '@/services/signalRService';


const Index = ({ itemData }: { itemData: Item }) => {
  const router = useRouter();
  const { user, accessToken } = useGlobalState(); // replace with your authentication context

  const [bidAmount, setBidAmount] = useState('');
  const [itemStatus, setItemStatus] = useState<string>(''); // 'started', 'not started', 'ended'
  const [winner, setWinner] = useState<User | null>(null);
  const [resMessage, setResMessage] = useState({
    content: "",
    color: ""
  });
  
  useEffect(() => {
    // Assuming you have a utility function to determine the item status
    const calculateItemStatus = (item: Item): string => {
      const currentDate = new Date();
      if (currentDate < parseDate(itemData.startDate)) {
        return 'not started';
      } else if (currentDate >= parseDate(itemData.startDate) && currentDate <= parseDate(itemData.endDate)) {
        return 'started';
      } else {
        return 'ended';
      }
    };

    setItemStatus(calculateItemStatus(itemData));

    setWinner(itemData?.auctionHistory?.winner ?? null);
   
  }, [itemData]);
  
  const handlePlaceBid = async () => {

    if (accessToken === "") {
      router.push("/auth/signin")
    }
    
    await signalRService.joinItemRoom(user.userId, itemData.itemId, parseInt(bidAmount, 10));

    await axiosService.post("/api/user/placeBid", {
      itemId: itemData.itemId,
      amount: bidAmount
    }).then((res) => {
      if (res.status == 200) {
        setResMessage({
          content: res.data?.message,
          color: "meta-4"
        })
      }
    }).catch((e) => {
      console.log(e?.response);
      if (e?.response?.status == 400) {
        setResMessage({
          content: e?.response?.data?.errors?.message,
          color: "meta-1"
        })        
      }
    })  
  }
  const isItemSeller = user && itemData?.seller?.userId === user.userId;
  
  return (
    <div className="flex flex-col md:flex-row p-4 gap-8">
      <div className="md:w-1/2">
        {itemData && (
          <>
            <img src={itemData.image} alt={itemData.title} className="mb-4" />
            <h1 className="text-3xl font-bold mb-4">{itemData.title}</h1>
            <p className="text-gray-600 mb-4">{itemData.description}</p>
            <p className="text-gray-600 mb-4">Seller: {itemData.seller?.name}</p>
            <p className="text-lg font-semibold mb-4">Price: ${itemData.startingPrice}</p>
            <p className="text-lg font-semibold mb-4">Increasing Amount: ${itemData.increasingAmount}</p>
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
              onClick={() => router.push(`/items/form?itemId=${itemData.itemId}`)}
              className="mt-2 px-4 py-2 bg-meta-5 hover:bg-meta-3 text-white rounded hover:bg-blue-600"
            >
              Edit
            </button>
          </div>
        )}

        {user && !isItemSeller && itemStatus === 'started' && (
          <div className="mb-8">
            <p  className={`text-${resMessage.color}`}>{resMessage.content}</p>
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
              onClick={handlePlaceBid}
              className="mt-2 px-4 py-2 bg-meta-5 hover:bg-meta-3 text-white rounded hover:bg-blue-600"
            >
              Place Bid
            </button>
          </div>
        )}

        {/* List of Bids */}
        <div>
          <h2 className="text-xl font-semibold mb-4">Placed Bids:</h2>
          <ul className="overflow-y-auto max-h-187.5">
            {!itemData?.bids || itemData?.bids.length == 0 && (
              <p>No Bid placed!</p>
            )}
            {itemData?.bids?.map((bid) => (
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