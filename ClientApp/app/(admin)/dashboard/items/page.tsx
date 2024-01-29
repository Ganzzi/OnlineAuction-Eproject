import ItemSellPage from "@/app/(home)/items/form/page";
import Breadcrumb from "@/components/Dashboard/Breadcrumb";
import ItemList from "@/components/Dashboard/items/ItemList";
import { SearchParams } from "@/types/next";

const ItemsPage = ({ searchParams }: { searchParams: SearchParams }) => {
  return (
    <>
      <Breadcrumb pageName="Items" />
      <ItemList />
    </>
  );
};

export default ItemsPage;
