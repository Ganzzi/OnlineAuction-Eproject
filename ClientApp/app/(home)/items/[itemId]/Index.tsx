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
import Link from 'next/link';
import { Countdown } from '@/components/Home/ItemCard';


const Index = ({ itemData }: { itemData: Item }) => {
  const router = useRouter();
  const { user, accessToken, isLoggedIn } = useGlobalState(); // replace with your authentication context

  const [bidAmount, setBidAmount] = useState('');
  const [itemStatus, setItemStatus] = useState<string>(''); // 'started', 'not started', 'ended'
  const [winner, setWinner] = useState<User | null>(null);
  const [resMessage, setResMessage] = useState({
    content: "",
    color: ""
  });

  const HighestBid = (itemData.bids && itemData.bids.length != 0 ? itemData?.bids[0]?.bidAmount : 0);
  const MinimumBid = itemData.increasingAmount + HighestBid;

  const endDate = itemData.auctionHistory?.winner != null
    ? itemData.auctionHistory.endDate
    : itemData.endDate;


  useEffect(() => {
    // Assuming you have a utility function to determine the item status
    const calculateItemStatus = (item: Item): string => {
      const currentDate = new Date();

      if (currentDate < parseDate(itemData.startDate)) {
        return 'not started';
      } else if (currentDate >= parseDate(itemData.startDate) && currentDate <= parseDate(endDate)) {
        return 'started';
      } else {
        return 'ended';
      }
    };

    setItemStatus(calculateItemStatus(itemData));


    setWinner(itemData?.auctionHistory?.winner ?? null);

  }, [itemData]);

  const handlePlaceBid = async () => {

    if (!isLoggedIn) {
      router.push("/auth/signin")

      return;
    }

    if (bidAmount == "") {
      setResMessage({
        content: "bid amount require",
        color: "meta-1"
      })
      return;
    }

    if (parseInt(bidAmount) < MinimumBid) {
      setResMessage({
        content: "require more than minimum bid",
        color: "meta-1"
      })
      return;
    }


    await axiosService.post("/api/user/placeBid", {
      itemId: itemData.itemId,
      amount: bidAmount
    }).then(async (res) => {
      if (res.status == 200) {
        await signalRService.joinItemRoom(user.userId, itemData.itemId, parseInt(bidAmount, 10));
        setResMessage({
          content: res.data?.message,
          color: "meta-4"
        })

        router.refresh();
      }
    }).catch((e) => {
      console.log(e?.response);
      if (e?.response?.status == 400) {
        setResMessage({
          content: e?.response?.data?.message,
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
            <div className='relative w-fit'>
              <div className={`-right-5 -top-5 absolute p-5 bg-meta-3 text-black-2 rounded-3xl`}>
                {itemStatus == "started" && (
                  <div>
                    <p className='text-font-bold text-lg'>Auction End in</p>
                    <Countdown date={endDate} />
                  </div>
                )}

                {itemStatus == "not started" && (
                  <div>
                    <p className='text-font-bold text-lg'>Auction Start in</p>
                    <Countdown date={itemData.startDate} />
                  </div>
                )}

                {itemStatus == "ended" && (
                  <div>
                    <p className='text-font-bold text-lg'>Auction Ended</p>
                  </div>
                )}
              </div>
              <img src={itemData.image} alt={itemData.title} className="mb-4" />
            </div>

            <div className='flex flex-row justify-between items-center'>

              <div>
                <h1 className="text-7xl font-bold mb-4">{itemData.title}</h1>
                <p className="text-lg text-body mb-4">{itemData.description}</p>
              </div>
              <div className='flex flex-row items-start justify-end'>
                <p className="text-gray-600 mb-4 text-3xl mx-3">{itemData.seller?.name}</p>
                <img src={itemData.seller?.avatar} className='w-16 h-16 rounded-full' alt="" />
              </div>
            </div>
            <Link
              href={`${process.env.NEXT_PUBLIC_SERVER_URL}/Upload/Files/${itemData.document}`}
              target='blank'
              className='text-lg text-secondary hover:text-meta-5'
            >
              download
            </Link>
            {winner != null && <p className="text-lg font-semibold mb-4">Winner: {winner.name}</p>}

            {/* Display other item details as needed */}
          </>
        )}
      </div>

      <div className="md:w-1/2">
        {/* Bid Form */}
        {user && isItemSeller && itemStatus !== 'ended' && (
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
            {itemData.reservePrice && (
              <p className='text-meta-3 text-center text-xl'>Preserve Price: {itemData.reservePrice}</p>
            )}
            <p className='text-meta-8'>Increasing Amount: {itemData.increasingAmount}</p>
            <p className='text-meta-8'>Highest Bid: {HighestBid}</p>
            <p className='text-meta-7'>* Current Minimum bid: {MinimumBid}</p>
            <label htmlFor="bidAmount" className="block text-sm font-medium text-gray-600">
              Your Bid Amount:
            </label>
            <input
              type="number"
              id="bidAmount"
              disabled={winner != null}
              value={bidAmount}
              onChange={(e) => {
                setBidAmount(e.target.value);

                if (parseInt(e.target.value) >= MinimumBid) { 
                  setResMessage({
                    color: "meta-3",
                    content: "be able to bid!"
                  })
                } else {
                  setResMessage({
                    content: "require more than minimum bid",
                    color: "meta-1"
                  })
                }
              }}
              className="mt-1 p-2 border rounded w-full"
            />
            <p className={`text-${resMessage.color}`}>{resMessage.content}</p>
            <button
              onClick={handlePlaceBid}
              disabled={winner != null}
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
            {itemData?.bids?.map((bid, key) => (
              <li key={bid.bidId} className="mb-2 relative">
                {key == 0 && (
                  <p className='right-5 -top-3 absolute p-5 bg-meta-3 text-black-2 rounded-3xl'>Highest Bid</p>
                )}
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