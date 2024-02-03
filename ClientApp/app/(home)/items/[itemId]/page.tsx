
import React from 'react';
import { Item } from '@/types/models/item';
import { item1, bid1, bid2, categoryItem1, categoryItem2, auctionHistory1 } from '@/data/item';
import Index from './Index';
import axiosService from '@/services/axiosService';
import { useRouter } from 'next/router';
import { useParams } from 'next/navigation';
import { Params } from 'next/dist/shared/lib/router/utils/route-matcher';
import Breadcrumb from '@/components/Home/Breadcrumb';
import axios from 'axios';
import https from 'https'

const itemData: Item = {
  ...item1,
  bids: [bid1, bid2, bid1, bid1, bid2, bid1, bid1, bid2],
  categoryItems: [categoryItem1, categoryItem2],
  auctionHistory: auctionHistory1
};

const fetchItemData: (itemId: number) => Promise<Item> = async (itemId: number) => {
  const url = new URL(process.env.NEXT_PUBLIC_SERVER_URL+'/api/User/getItemById')
  url.searchParams.set("id", itemId.toString());

  const response = await axios.get(url.toString(), {
    httpsAgent: new https.Agent({ rejectUnauthorized: false }), // Ignore SSL certificate validation errors
  });

  const itemData: Item = response.data;

  return itemData;
}

export default async function  ItemDetailPage (
      { params: {itemId} }: { params: {itemId: number} }
    ) {
  const itemData = await fetchItemData(itemId);
  itemData.bids = itemData.bids?.reverse();
  
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

