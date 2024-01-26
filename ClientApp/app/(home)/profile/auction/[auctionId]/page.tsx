'use client'

import axiosService from '@/services/axiosService';
import { useGlobalState } from '@/context/globalState';
import { AuctionHistory } from '@/types/models/auctionHistory'
import { SearchParams } from '@/types/next'
import { parseDate } from '@/utils';
import { useParams, useRouter } from 'next/navigation';
import React, { useEffect, useState } from 'react'

type RateBuyerRequest = {
  ItemId: number,
  RatedUserId: number,
  RatingAmount: number,
}

const AuctionHistoryPage = ({ params: { auctionId } }: { params: { auctionId: number } }) => {
  const { user } = useGlobalState();
  const router = useRouter();

  const [auctionData, setAuctionData] = useState<AuctionHistory>();
  const [ratingAmount, setRatingAmount] = useState<number>(0);
  const [errors, setErrors] = useState({
    message: "",
    ratingAmount: "",
  })

  useEffect(() => {
    const fetchAuctionData = async () => {
      try {
        const res = await axiosService.get(`/api/user/AuctionHistoryDetail?AuctionHistoryId=${auctionId}`);
        const data: AuctionHistory = res.data;

        setAuctionData(data);
      } catch (error) {
        console.error('Error fetching auction data', error);
      }
    };

    fetchAuctionData();
  }, [auctionId]);

  const handleRateBuyer = async () => {

    // Create RateBuyerRequest object
    const rateBuyerRequest: RateBuyerRequest = {
      ItemId: auctionData?.itemId || 0,
      RatedUserId: auctionData?.winnerId || 0,
      RatingAmount: ratingAmount,
    };

    // Post to /api/user/ratebuyer
    await axiosService.post('/api/user/ratebuyer', rateBuyerRequest)
      .then((res) => {
        if (res.status == 200) {
          router.push(`/items/${auctionData?.itemId}`);
        }
      })
      .catch((e) => {
        if (e?.response?.status == 400) {
          setErrors({
            message: e?.response?.data?.errors?.message,
            ratingAmount: e?.response?.data?.errors?.ratingAmount,
          });
        }
      });
  };

  return (
    <div className="container mx-auto p-8">
      <h2 className="text-3xl font-bold mb-4">Auction History Page</h2>

      {auctionData && (
        <div className="bg-gray-100 p-4 rounded-md shadow-md text-xl flex flex-row justify-center space-x-14 items-start">
          <div className='flex flex-col'>
            <p>Winning Bid: ${auctionData.winningBid}</p>
            <p>End Date: {parseDate(auctionData.endDate).toDateString()}</p>
            <p>Winner: {auctionData.winner ? auctionData.winner?.name : "None"}</p>
          </div>

          {auctionData?.item?.sellerId === user.userId ? (
            <>
              {auctionData.item.rating != null ? (
                <div>
                  <p className='text-meta-3 text-2xl'>You rated {auctionData.winner?.name} {auctionData.item.rating.rate} star</p>
                </div>
              ) : (
                <div className="items-center flex flex-col">
                  <p className='text-meta-3'>{errors.message}</p>
                  <p className='text-meta-1'>{errors.ratingAmount}</p>
                  <label className="block mb-2">
                    Rating Amount:
                    <input
                      disabled={auctionData.winner == null ? true : false}
                      type="number"
                      value={ratingAmount}
                      onChange={(e) => setRatingAmount(parseInt(e.target.value, 10))}
                      className="border border-gray-300 px-2 py-1 rounded-md focus:outline-none focus:border-blue-500"
                    />
                  </label>
                  <button
                    disabled={auctionData.winner == null ? true : false}
                    onClick={handleRateBuyer}
                    className="bg-meta-5 text-white px-4 py-2 rounded-md hover:bg-meta-3 focus:outline-none focus:bg-blue-700"
                  >
                    Rate Buyer
                  </button>
                </div>
              )}
            </>

          ) : (
            <div className='flex flex-col items-center'>
              <p className="mt-4">Seller Email: {auctionData.item?.seller?.email}</p>
              <button
                className="bg-meta-6 text-white px-4 py-2 rounded-md hover:bg-meta-7 focus:outline-none focus:bg-green-700"
              >
                Contact
              </button>
            </div>
          )}
        </div>
      )}

      {/* Render Item Information */}
      {auctionData?.item && (
        <div className="mt-8">
          <div className='flex flex-row items-center space-x-5'>
            <h3 className="text-2xl font-bold mb-4">Item Information</h3>
            <button
              onClick={() => router.push(`/items/${auctionData.itemId}`)}
              className="bg-meta-5 text-white px-4 py-2 rounded-md hover:bg-primary focus:outline-none focus:bg-green-700"
            >
              View Item
            </button>
          </div>
          <div className="flex items-center space-x-4 text-lg">
            <img src={auctionData.item.image} alt={auctionData.item.title} className="w-32 h-32 object-cover rounded-md" />
            <div>
              <p className="font-bold">{auctionData.item.title}</p>
              <p className="text-gray-600">{auctionData.item.description}</p>
              <p>Starting Price: {auctionData.item.startingPrice}</p>
              <p>Increasing Amount: {auctionData.item.increasingAmount}</p>
              {/* Add other item details as needed */}
            </div>
          </div>
        </div>
      )}
    </div>
  );

};

export default AuctionHistoryPage