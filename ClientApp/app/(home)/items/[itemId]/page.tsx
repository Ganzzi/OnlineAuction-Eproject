
import React from 'react';
import { Item } from '@/types/models/item';
import { item1, bid1, bid2, categoryItem1, categoryItem2 } from '@/data/item';
import Index from './Index';
import axiosService from '@/axiosService';
import { useRouter } from 'next/router';
import { useParams } from 'next/navigation';
import { Params } from 'next/dist/shared/lib/router/utils/route-matcher';

const itemData: Item = {
  ...item1,
  bids: [bid1, bid2, bid1, bid1, bid2, bid1, bid1, bid2],
  categoryItems: [categoryItem1, categoryItem2],
};

const fetchItemData: (itemId: number) => Promise<Item> = async (itemId: number) => {
  // await axiosService.get("");

  return itemData;
}

export default async function  ItemDetailPage (
      { params: {itemId} }: { params: {itemId: number} }
    ) {
  const itemData = await fetchItemData(itemId);
  
  return <Index itemData={itemData}/>
};

