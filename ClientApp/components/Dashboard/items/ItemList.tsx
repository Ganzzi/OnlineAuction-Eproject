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
import { ItemData } from '@/app/(admin)/dashboard/items/page';

const ItemList = ({ data }: { data: ItemData[] }) => {

  return (
    <div>
     <br />
     <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
    
      <div className="grid grid-cols-6 border-t border-stroke py-4.5 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-7.5">
        <div className="col-span-1 flex items-center">
          <p className="font-medium">Id</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">image</p>
        </div>
        <div className="col-span-1 hidden items-center sm:flex">
          <p className="font-medium">title</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">description</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">End</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">seller</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">Categories</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">Bids</p>
        </div>
      </div>

      {data.map((product, key) => (
        <ItemCard product={product} key={key}/>
      ))}
    </div>
    </div>
  )
}

const ItemCard = ({product}: {product: ItemData}) => {
  const router = useRouter();
  return (
    <div onClick={()=>{
      router.push(`/dashboard/items/${product.item.itemId}`)}} 
      className="hover:bg-meta-3 grid grid-cols-6 border-t border-stroke py-4.5 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-7.5"
    >
      <div className="col-span-1 flex items-center">
        <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
          <div className="h-12.5 w-15 rounded-md">
          {product.item.itemId}
          </div>
        </div>
      </div>
      <div className="col-span-1 flex items-center">
        <img src={product.item.image} className="w-24 h-24" alt="" />
      </div>
      <div className="col-span-1 hidden items-center sm:flex">
        <p className="text-sm text-black dark:text-white">
          {product.item.title}
        </p>
      </div>
      <div className="col-span-1 hidden items-center sm:flex">
        <p className="text-sm text-black dark:text-white">
          {product.item.description}
        </p>
      </div>
      <div className="col-span-1 flex items-center">
        <p className="text-sm text-black dark:text-white">{new Date(product.item.endDate).toDateString()}</p>
      </div>
      <div className="col-span-1 flex items-center">
        <p className="text-sm text-meta-3">{product.item.seller?.name}</p>
      </div>
      <div className="col-span-1 flex items-center">
        <p className="text-sm text-meta-3">{product.categories}</p>
      </div>
      <div className="col-span-1 flex items-center">
        <p className="text-sm text-meta-3">{product.bidCount}</p>
      </div>
    </div>
  )
}

export default ItemList