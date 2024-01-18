
import React from 'react';
import { Item } from '@/types/models/item';
import { item1, bid1, bid2, categoryItem1, categoryItem2, auctionHistory1 } from '@/data/item';
import Index from './Index';
import axiosService from '@/axiosService';
import { useRouter } from 'next/router';
import { useParams } from 'next/navigation';
import { Params } from 'next/dist/shared/lib/router/utils/route-matcher';
import Breadcrumb from '@/components/Home/Breadcrumb';

const itemData: Item = {
  ...item1,
  bids: [bid1, bid2, bid1, bid1, bid2, bid1, bid1, bid2],
  categoryItems: [categoryItem1, categoryItem2],
  auctionHistory: auctionHistory1
};

const fetchItemData: (itemId: number) => Promise<Item> = async (itemId: number) => {
  // await axiosService.get("");

  return itemData;
}

export default async function  ItemDetailPage (
      { params: {itemId} }: { params: {itemId: number} }
    ) {
  const itemData = await fetchItemData(itemId);
  
  return (
  <>
  <Breadcrumb listPages={[
    {
      pageName: "market",
      link: "/items/"
    },{
      pageName: `${itemData.title} - ${itemId}` ,
      link: "/items/"+itemId
    },
    ]} />
  <Index itemData={itemData}/>
  </>
  )
};

