"use client"
import React from 'react'
import { Resource, newResource } from "@/types/resource";
import { SearchParams } from "@/types/next";
import Pagination from "@/components/common/Pagination/Pagination";
import { Item } from '@/types/models/item';
import { User } from '@/types/models/user';
import { CategoryItem } from '@/types/models/categoryItem';
import { Bid } from '@/types/models/bid';
import { useRouter } from 'next/navigation';
const user: User[]=[];
const categoryItems: CategoryItem[]=[];
const bid: Bid[]=[];
const item: Item[] = [
  {
    itemId: 1,
    title: "2",
    description: "3",
    imgUrl: "4",
    price: 5,
    sellerId: 6,
    seller: user[1],
    categoryItems: categoryItems,
    bids: bid,
  }
];
type Props = {}


const ItemList = ({ searchParams }: { searchParams: SearchParams }) => {
  const resource = newResource([], 93, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10);
  const router = useRouter();
  return (
    <div>
     <Pagination meta={resource.meta} />
     <br />
     <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
    
      <div className="grid grid-cols-6 border-t border-stroke py-4.5 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-7.5">
        <div className="col-span-1 flex items-center">
          <p className="font-medium">Id</p>
        </div>
        <div className="col-span-1 hidden items-center sm:flex">
          <p className="font-medium">title</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">description</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">imgUrl</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">price</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">seller</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">Category</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">bids</p>
        </div>
      </div>

      {item.map((product, key) => (
        <div onClick={()=>{
          router.push(`/dashboard/items/${product.itemId}`)}} 
          className="grid grid-cols-6 border-t border-stroke py-4.5 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-7.5"
          key={key}
        >
          <div className="col-span-1 flex items-center">
            <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
              <div className="h-12.5 w-15 rounded-md">
              {product.itemId}
              </div>
            </div>
          </div>
          <div className="col-span-1 hidden items-center sm:flex">
            <p className="text-sm text-black dark:text-white">
              {product.title}
            </p>
          </div>
          <div className="col-span-1 hidden items-center sm:flex">
            <p className="text-sm text-black dark:text-white">
              {product.description}
            </p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="text-sm text-black dark:text-white">
              {product.imgUrl}
            </p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="text-sm text-black dark:text-white">${product.price}</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="text-sm text-meta-3">{product.seller?.username}</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="text-sm text-meta-3">{product.categoryItems?.length}</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="text-sm text-meta-3">{product.bids?.length}</p>
          </div>
        </div>
      ))}
    </div>
    </div>
  )
}

export default ItemList