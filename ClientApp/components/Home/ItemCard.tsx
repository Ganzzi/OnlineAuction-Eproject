import useColorMode from '@/hooks/useColorMode';
import { Bid } from '@/types/models/bid';
import { Item } from '@/types/models/item';
import { useRouter } from 'next/navigation';
import React from 'react'


const ItemCard = ({ item }: { item: Item }) => {
    const router = useRouter()
  
    return (
      <div className="bg-white shadow-md rounded-lg p-4 transition-transform transform hover:scale-105"
        onClick={() => router.push(`/items/${item.itemId}`)}
      >
        <h5 className="text-xl font-bold mb-2">{item.title}</h5>
        <p className="text-gray-600 mb-4">{item.description}</p>
        <img src={item.imgUrl} alt={item.title} className="w-full mb-4" />
        <p className="text-gray-600 mb-4">${item.price}</p>
        {item.sellerId && (
          <div className="flex items-center mb-4">
            <p className="text-gray-600 mb-2">{item.seller?.username}</p>
          </div>
        )}
      </div>
    );
  };
  
export default ItemCard