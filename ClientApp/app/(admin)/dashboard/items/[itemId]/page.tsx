'use client'

import Breadcrumb from "@/components/Dashboard/Breadcrumb";
import CategoryList from "@/components/Dashboard/items/CategoryList";
import axiosService from "@/services/axiosService";
import { Category } from "@/types/models/category";
import { Item } from "@/types/models/item";
import Link from "next/link";
import { useEffect, useState } from "react";

export type CategoryData = {
  category: Category,
  belong:  boolean
}

type DataResponse  = {
  item: Item,
  categories: CategoryData[]
}

const ItemDetailPage = ({ params: { itemId } }: { params: { itemId: number } }) => {
  const [data, setData] = useState<DataResponse | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await axiosService.get(`/api/Admin/ItemDetailWithListCategoryItems/${itemId}`);
        const _data: DataResponse = res.data;
        setData(_data);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, [itemId]);

  return (
    <>
      <Breadcrumb pageName="ItemDetail" />

      {data && (
        <div className="max-w-2xl mx-auto p-4 space-y-4">
        {/* Render item's base info */}
        <div className="mb-8">
          <div className="flex flex-row items-center justify-start">

          <div>
            <h2 className="text-2xl font-bold mb-2">{data.item.title}</h2>
            <p className="text-gray-500">{data.item.description}</p>
          </div>

          <img src={data.item.image} className="w-20 h-20 object-cover rounded-md" alt="Item" />
          </div>

          <div className="grid grid-cols-2 gap-4 mt-4">
            <div>
              <p>Starting Price: ${data.item.startingPrice}</p>
              <p>Increasing Amount: ${data.item.increasingAmount}</p>
            </div>
            <div>
              <p>Start Date: {new Date(data.item.startDate).toDateString()}</p>
              <p>End Date: {new Date(data.item.endDate).toDateString()}</p>
            </div>
          </div>
        </div>

        {/* Item Image */}

        {/* Show list of categories */}
        <CategoryList categories={data.categories} itemId={itemId}/>
      </div>
      )}
    </>
  );
};


export default ItemDetailPage;
