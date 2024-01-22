import { Resource } from '@/types/resource';
import React from 'react'
import Paginate from '../common/Pagination/Paginate';
import RowsPerPage from '../common/Pagination/RowsPerPage';
import Summary from '../common/Pagination/Summary';

type Props = {
  meta: Resource<unknown>['meta'];
  setPerPage?: (perPage: number) => void;
} & Pick<Parameters<typeof Paginate>[0], 'setPage'>
& Pick<Parameters<typeof RowsPerPage>[0], 'setPerPage'>

export default function Pagination(props: Props) {
  const {
    meta: {
      from,
      to,
      total,
      per_page: perPage,
      last_page: lastPage,
      current_page: currentPage,
    },
    setPerPage,
    setPage,
  } = props

  return (
    <div className="flex flex-col items-center justify-content-center">
      <Summary from={from} to={to} total={total} />
      <Paginate currentPage={currentPage} lastPage={lastPage} setPage={setPage} />
    </div>
  )
}
