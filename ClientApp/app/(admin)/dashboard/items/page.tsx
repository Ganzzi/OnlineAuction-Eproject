'use client';

import Breadcrumb from "@/components/Dashboard/Breadcrumb";
import ItemList from "@/components/Dashboard/items/ItemList";
import Pagination from "@/components/Dashboard/pagination";
import axiosService from "@/services/axiosService";
import { Item } from "@/types/models/item";
import { SearchParams } from "@/types/next";
import { Resource, newResource } from "@/types/resource";
import { useEffect, useState } from "react";

export type ItemData = {
  item: Item,
  categories: number,
  bidCount: number
}

const ItemsPage = ({ searchParams }: { searchParams: SearchParams }) => {
  const [resource, setResource] = useState<Resource<ItemData>>(newResource([], 10, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10))

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await axiosService.get(`/api/Admin/GetListItems${searchParams?.page != undefined ? "?page=" + searchParams?.page : ""}`);

        const _data: ItemData[] = res.data.listItem;
        const count = res.data.countpage;

        const src = newResource(_data, count, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10);

        setResource(src);
      } catch (error) {
        console.error('Error fetching categories:', error);
        // Handle the error or provide a fallback mechanism if needed
      }
    };

    fetchData();
  }, [searchParams?.page]);

  return (
    <>
      <Breadcrumb pageName="Items" />
      <Pagination meta={resource.meta} />
      <br />
      <ItemList data={resource.data} />
    </>
  );
};

export default ItemsPage;
